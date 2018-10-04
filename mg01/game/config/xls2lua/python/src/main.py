# __author__ = jr.zeng
# -*- coding:utf-8 -*

print("enter main")

import sys
import os
import shutil
import json

import xlrd
import collections

default_encoding = 'utf-8'
if sys.getdefaultencoding() != default_encoding:
    # if not sys.stdout.encoding:
    reload(sys)
    sys.setdefaultencoding(default_encoding)

from util import *
from os.path import join

# global PATH_CFG  # 配置表路径
PATH_RUN = os.getcwd()  # 运行目录
PATH_MAIN = sys.argv[0]  # 主程序路径
print("run path is: %s" % PATH_RUN)
print("script path is: %s" % PATH_MAIN)

is_client = list_util.get(sys.argv, 1) != "is_server"  # 是否客户端导表
use_skip = list_util.get(sys.argv, 2) != "no_skip"  # 快捷导表模式

ROW_TYPE = 0  # 第一行：type
ROW_NAME = 1  # 第二行：变量名
ROW_TITLE = 2  # 第三行：中文名
ROW_VAL = 3


# 合法的类型标签
class type_flag():
    default = 'default'
    key = 'key'  # 这个是key
    ignored = 'ignored'  # 忽略此列
    server = 'server'  # 服务端专用
    client = 'client'  # 客户端专用


class formatstring(str):
    def __new__(cls, value):
        return str.__new__(cls, value)

    def __init__(self, value):
        pass


# 支持的类型
# 基本类型 int, float, string, format_string, bool
# 结构类型 list, xlist, struct, xstruct, dict

# 基础类型
type_tbl = {
    'int': int,
    'float': float,
    'string': str,
    'formula': str,
    'format_string': formatstring,
    'bool': bool,
}

type2default = {
    'int': 0,
    'float': 0.0,
    'string': '',
    'formula': '',
    'format_string': formatstring(''),
    'bool': False,
}


# 是否struct类型
def is_struct_type(val_type):
    return val_type.startswith('struct') or val_type.startswith('xstruct')


# 获取类型"list<xxx>"的xxx
def get_type_list_val(type_col):
    return type_col.split('<')[1].split('>')[0]


# 检测类型值的合法性
def check_type_val_legal(type_val):
    if type_val not in type_tbl and not is_struct_type(type_val):
        if not type_val in enum_tbl and not str(type_val) in enum_tbl:
            assertx(False, '未知的类型 :[%s]' % type_val)


# 检测列表类型值的合法性
def check_type_list_legal(type_col):
    type_val = get_type_list_val(type_col)
    check_type_val_legal(type_val)


# 强转为int
def force_int(val):
    try:
        try:
            ival = int(val)
        except:
            ival = int(eval(val))  # 将字符串str当成有效的表达式来求值并返回计算结果
        try:
            fval = float(val)
        except:
            fval = eval(val)
        assertx(abs(fval - ival) < 0.0001, "要求int,可能填入 float(int require):%s" % val)  # 通过差值判断是否整形
        return ival
    except:
        raise AssertionError, str("非法的int(invalid int value):%s" % val).encode('utf-8')


def is_xls_file(path):
    if not os.path.isfile(path):
        return False
    root, ext = os.path.splitext(path)
    if ext != '.xls':
        return False
    file_name = os.path.basename(path)
    if file_name.startswith('_'):  # 不导出
        return False
    return True


# --//-------∽-★-∽------∽-★-∽--------∽-★-∽lua生成∽-★-∽--------∽-★-∽------∽-★-∽--------//

# 三引号支持换行
lua_temple_begin = """
return {
"""
lua_temple_row_begin = """
    [%s] = {
"""
lua_temple_cell = """\
        %s = %s,
"""
lua_temple_row_end = """\
    },
"""
lua_temple_end = """
}
"""
lua_temple_map_flag = """
______IS__MAP______ = true,"""  # 字典标志

lua_temple_desc_flag = """
______DESC______ = "%s","""


# 转lua格式
def value_to_lua(value):
    if isinstance(value, basestring):
        if type(value) == formatstring:
            form = '[=[%s]=]'
            return form % value
        else:
            if '\n' in value:
                form = '[=[%s]=]'
            elif '"' in value:
                form = '[[%s]]' if "'" in value else "'%s'"
            else:
                form = '"%s"'
            return form % value
    elif isinstance(value, bool):
        if value:
            return "true"
        else:
            return "false"
    elif isinstance(value, list):
        value = ', '.join([value_to_lua(v) for v in value])
        value = '{%s}' % value

    elif isinstance(value, dict):
        value = ', '.join(["[%s] = %s" % (value_to_lua(k), value_to_lua(v)) for k, v in value.items()])
        value = '{%s}' % value

    elif isinstance(value, tuple):
        # 枚举值
        type_ = decode_std(value[0])  # type_转unicode
        value = "Enum.get('%s', '%s')" % (type_, value[1])

    return str(value)


# sheet生成lua文件
def sheet_to_lua(sheet_name, raw_gen, desc, output):
    if not os.path.exists(output):
        os.mkdir(output)

    lua_name = join(output, sheet_name + '.lua')
    with open(lua_name, 'wb') as file:

        comment = '-- %s' % desc
        file.write(comment.encode('utf-8'))
        file.write(lua_temple_begin.encode('utf-8'))  # return {
        is_map = None  # 是否字典
        for col, row_data in enumerate(raw_gen):

            main_key = sheet2mainKey.get(sheet_name, 'id')  # 相当于sheet2mainKey[sheet_name] or sheet2mainKey['id']
            if is_map is None and row_data.get(main_key) is not None:
                is_map = True

            main_key_value = row_data.get(main_key, col + 1)  # 没有id的值则用序号做id

            if isinstance(main_key_value, basestring):
                main_key_value = value_to_lua(main_key_value)
            elif isinstance(main_key_value, tuple):
                main_key_value = value_to_lua(main_key_value)  # 主键值是枚举

            file.write((lua_temple_row_begin % main_key_value).encode('utf-8'))  # [id] = {

            for key, value in row_data.items():
                if isinstance(value, basestring):
                    value = value_to_lua(value)
                elif isinstance(value, bool):
                    value = value_to_lua(value)
                elif isinstance(value, list):
                    value = ', '.join([value_to_lua(v) for v in value])
                    value = '{%s}' % value
                elif isinstance(value, dict):
                    value = ', '.join(["[%s] = %s" % (value_to_lua(k), value_to_lua(v)) for k, v in value.items()])
                    value = '{%s}' % value
                elif isinstance(value, tuple):
                    value = value_to_lua(value)

                cell = lua_temple_cell % (key, value)  # a = 1,
                file.write(cell.encode('utf-8'))
            file.write(lua_temple_row_end.encode('utf-8'))

        if is_map:
            file.write(lua_temple_map_flag.encode('utf-8'))
        else:
            pass

        file.write((lua_temple_desc_flag % desc).encode('utf-8'))

        file.write(lua_temple_end.encode('utf-8'))  # }
    return lua_name


def enum_to_lua(enum_name, name2idValue, desc, output):
    # print('enum_to_lua %s' % enum_name)
    lua_name = join(output, enum_name + '.lua')
    with open(lua_name, 'wb') as file:
        comment = '-- %s\n' % desc
        file.write(comment.encode('utf-8'))
        file.write(lua_temple_begin.encode('utf-8'))
        for name, id2value in name2idValue.items():
            file.write(("[%s] = %s,\n" %
                        (value_to_lua(name), value_to_lua(id2value))).encode('utf-8'))
        file.write(lua_temple_end.encode('utf-8'))
    return lua_name


# --//-------∽-★-∽------∽-★-∽--------∽-★-∽xls解析∽-★-∽--------∽-★-∽------∽-★-∽--------//

# 获取枚举值
def get_enum_value(type_, cell_value):
    result = None

    # type_ 可能是unicode或者是string，这里都尝试一下
    if enum_tbl.has_key(type_):
        key2value = enum_tbl[type_]
    elif enum_tbl.has_key(str(type_)):
        key2value = enum_tbl[str(type_)]

    if key2value:
        if key2value.has_key(cell_value):
            result = key2value[cell_value]
        elif key2value.has_key(str(cell_value)):
            result = key2value[str(cell_value)]

    if result is None:

        try:
            cell_value = float(cell_value)
            cell_value = int(cell_value)
            # cell_value = unicode(cell_value)  #最后转unicode傻阿
        except:
            pass

        try:
            result = int(cell_value)
        except:
            assertx(False, '未定义枚举值 [%s]:[%s]' % (type_, cell_value))
            pass

        if result not in key2value.values():  # no_name的情况，纯粹判断cell_value的合法性，不是name转id
            assertx(False, '未定义枚举值 [%s]:[%s]' % (type_, cell_value))
    return result


# 解析单元格
def cell_to_value(type_, cell_value):
    result = None

    if type_.startswith('xstruct'):  # xstruct(int[id]|int[rate])
        tmp = type_.split("(")[1].split(")")[0]  # 'int[id]|int[rate]'
        tmps = tmp.split("|")  # 'int[id]','int[rate]'
        cell_value = str(cell_value)
        cell_value = cell_value.strip()
        values = cell_value.split("|")  # cell里的值列表

        key2value = {}
        for i, j in enumerate(tmps):
            if i >= len(values):  # 值缺省，允许
                break

            if j.find("[") != -1 and j.find("]") != -1:  # j:int[id]
                sub_type = j.split("[")[0]  # int
                sub_key = j.split("[")[1].split("]")[0]  # rate
            else:
                sub_type = j  # int：这种规则struct(int|int)
                sub_key = i + 1  # 键值为序号
            key2value[sub_key] = cell_to_value(sub_type, values[i])

        result = key2value

    elif type_.startswith('struct'):  # struct(int[id]|int[rate])
        tmp = type_.split("(")[1].split(")")[0]  # 'int[id]|int[rate]'
        tmps = tmp.split("|")  # 'int[id]','int[rate]'
        cell_value = str(cell_value)
        cell_value = cell_value.strip()
        values = cell_value.split("|")  # cell里的值列表

        key2value = {}
        for i, j in enumerate(tmps):
            if i >= len(values):  # 值缺省，不允许
                raise Exception('ERROR: struct 值数量错误, %s: %s' % (type_, cell_value))
                break

            if j.find("[") != -1 and j.find("]") != -1:
                sub_type = j.split("[")[0]  # int
                sub_key = j.split("[")[1].split("]")[0]  # rate
            else:
                sub_type = j  # int：这种规则struct(int|int)
                sub_key = i + 1  # 键值为序号
            key2value[sub_key] = cell_to_value(sub_type, values[i])

        result = key2value

    elif type_.startswith('list'):  # list<int>
        sub_type = type_.split("<")[1].split(">")[0]
        cell_value = str(cell_value)
        cell_value = cell_value.strip()
        values = cell_value.split(",")  # cell里的值列表
        key2value = {}
        for i, value in enumerate(values):
            key = i + 1  # 下标从1开始
            key2value[key] = cell_to_value(sub_type, value)

        result = key2value

    elif type_ in type_tbl:

        if type_ == 'int':
            result = force_int(cell_value)
        elif type_ == 'bool':
            if not isinstance(cell_value, bool):  # 可能传入的就是bool型
                if isinstance(cell_value, basestring):  # 支持填true/false
                    if cell_value.lower() == "true":
                        result = True
                    elif cell_value.lower() == "false":
                        result = False

                if result is None:
                    try:
                        value = int(cell_value)
                        if value == 0:
                            result = False
                        elif value == 1:
                            result = True
                        else:
                            errmsg = "不合法的bool值，可选[true | false | 0 | 1] 实际值->(%s)" % value
                            print(errmsg)
                            raise Exception(errmsg)
                    except:
                        raise

        elif type_ == 'float':
            try:
                result = float(cell_value)
            except:
                result = eval(cell_value)  # 求值

        elif type_ == 'string':
            try:  # 这里不懂
                fv = int(cell_value)
                if fv == cell_value:
                    cell_value = fv
            except:
                pass

            result = str(cell_value)

        elif type_ == 'format_string':
            # try:                  #  这里不懂
            #     fv = int(cell_value)
            #     if fv == cell_value:
            #         value = fv
            # except:
            #     pass

            result = formatstring(cell_value)
        else:
            result = type_tbl[type_](cell_value)  # 强转类型

    else:  # 不在type_tbl里

        if use_skip:
            result = (type_, cell_value)  # 枚举用tuple
        else:
            result = get_enum_value(type_, cell_value)

    return result


sheet2mainKey = {}  # 页签名->主key
enum_tbl = {}  # 枚举名称2dic(key2id)
empty_row = set([''])  # 判断空行


# 解析页签
def sheet_to_data(sheet):
    type_row = sheet.row_values(ROW_TYPE)  # 第一行：type
    name_row = sheet.row_values(ROW_NAME)  # 第二行：变量名
    title_row = sheet.row_values(ROW_TITLE)  # 第三行：中文名
    if len(type_row) == 0 or len(name_row) == 0:
        print('sheet type or name row not exist')
        return
    if ((not type_row[0]) or type_row[0] == '') or ((not name_row[0]) or name_row[0] == ''):
        print('sheet type or name row invalid ')
        return

    main_key = None  # 这个sheet的键名
    ignored_names = []  # 忽略的变量名
    only_client_names = []  # 仅客户端使用的字段
    only_server_names = []  # 仅服务器使用的字段

    col2type = {}

    for col, type_col in enumerate(type_row):
        try:
            if string_util.is_null_or_empty(type_col):  # 到这里断了，后面不再解析
                if not string_util.is_null_or_empty(name_row[col]):  # 如果名称有填就警告一下
                    print('info: 第%d列类型为空, 忽略之后的列' % (col + 1))
                type_row = type_row[:col]
                break

            type_ = type_col  # 字段类型

            # 检测类型标志
            if type_col.find('@') != -1:
                splits = type_col.split('@')
                type_ = splits[0]
                splits = splits[1:]
                for flag in splits:
                    assertx(type_flag.__dict__.has_key(flag),
                            '@illegal type flag %s [%s] ' % (name_row[col], flag))
                    if flag == type_flag.ignored:
                        ignored_names.append(name_row[col])
                    elif flag == type_flag.key:
                        main_key = name_row[col]
                        sheet2mainKey[sheet.name] = main_key
                    elif flag == type_flag.server:
                        # 字段仅服务器使用
                        only_server_names.append(name_row[col])
                    elif flag == type_flag.client:
                        # 字段仅客户端使用
                        only_client_names.append(name_row[col])

            # 检测类型的合法性
            if type_col.startswith('list'):
                check_type_list_legal(type_col)
                type_ = 'list'
            elif type_col.startswith('xlist'):
                check_type_list_legal(type_col)
                type_ = 'xlist'
            elif type_col.startswith('dict'):
                check_type_list_legal(type_col)
                type_ = 'dict'
            else:
                check_type_val_legal(type_)

            col2type[col] = type_  # 记录每一列的类型
        except:
            print >> sys.stderr.write('ERROR: row:%s column:[%s] \n' % (col + 1, type_col))
            raise
    # end for

    name2col = {}
    name2cols_list = collections.defaultdict(list)  # 名称为list的字典

    for col, name in enumerate(name_row):
        if name.find('|') != -1:
            name2cols_list[name].append(col)
        else:
            name2col[name] = col

    mainKeySet = {}  # 判断重复键值

    for row in range(ROW_VAL, sheet.nrows):

        val_row = sheet.row_values(row)  # 行数据(值)
        if set(val_row) == empty_row:  # 空行，忽略后面所有行
            break

        for col, type_col in enumerate(type_row):  # 遍历类型
            type_ = col2type[col]
            cell_value = val_row[col]  # 单元格的值

            if type_ == 'list':  # 2,6,10
                if cell_value == '':
                    cell_value = []
                else:
                    cell_value = str(cell_value)
                    type_val = get_type_list_val(type_col)  # 列表里的类型
                    values = [val.strip() for val in cell_value.split(',')]  # 对每个val去前后空格后的数组
                    cell_value = [cell_to_value(type_val, val) for val in values]  # 转换后的值列表
            elif type_ == 'xlist':  # 2|6|10
                if cell_value == '':
                    cell_value = []
                else:
                    cell_value = str(cell_value)
                    type_val = get_type_list_val(type_col)  # 列表里的类型
                    values = [val.strip() for val in cell_value.split('|')]  # 对每个val去前后空格后的数组
                    cell_value = [cell_to_value(type_val, val) for val in values]  # 转换后的值列表
            elif type_ == 'dict':
                if cell_value == '':
                    cell_value = []
                else:
                    cell_value = str(cell_value)
                    type_val = get_type_list_val(type_col)  # 列表里的类型
                    values = [val.strip() for val in cell_value.split(',')]  # 对每个val去前后空格后的数组
                    lsts = [cell_to_value(type_val, val) for val in values]  # type是struct且有id
                    cell_value = {val['id']: val for val in lsts}  # {[1]:val1,[2]:val2} 转为字典

            elif cell_value == '' and (('@default' in type_col) or ('@ignored' in type_col)):
                # cell没填值，且有default标签
                if type_ in type_tbl:
                    cell_value = type2default[type_]  # 取默认值
                elif is_struct_type(type_):
                    cell_value = {}
                else:
                    cell_value = 0

            else:
                # 其他情况
                assertx(cell_value != '', 'line[%s] column[%s] can not be empty' % (row + 1, title_row[col]))
                cell_value = cell_to_value(type_, cell_value)
                if name_row[col] == main_key:
                    # 这列是主键的话，保证值不能有重复
                    assertx(cell_value not in mainKeySet,
                            ' duplicate key %s row[%s] col[%s]' % (title_row[col], row, col))
                    mainKeySet[cell_value] = True

            val_row[col] = cell_value  # 一行里每列的新值赋回去
        # end for

        raw_data = {}  # {id:1, name:"小魔怪", desc:"这是描述"}
        for name, col in name2col.iteritems():

            if name == '':
                continue
            if name in ignored_names:
                continue
            # 导服务器的时候忽略仅有客户端的字段
            if (not is_client) and (name in only_client_names):
                continue
            # 导客户端的时候忽略仅为服务器的字段
            if is_client and (name in only_server_names):
                continue

            if len(type_row) > col:  # 仅取类型列表的范围
                raw_data[name] = val_row[col]
        # end for

        yield raw_data  # 返回generator, 将在下次递归时继续执行
    # end for


# end def


def excute_convert(xls_path, out_path, sheet2path):
    print('convert xls:[%s]' % (decode_sys(xls_path)))

    workbook = xlrd.open_workbook(xls_path)  # , encoding_override='gbk')
    lua_list = []
    for sheet in workbook.sheets():
        if sheet.nrows == 0:
            continue
        if sheet.name.startswith('_'):
            print('\t-- ignored:[%s]' % (sheet.name))
            continue

        print('\tconverting...:[%s]' % (sheet.name))
        # try:
        if sheet.name in sheet2path:
            old_path = sheet2path[sheet.name]
            # assertx(False, 'duplicate sheet name:[%s]' % old_path)
            assertx(False, '页签名重复')

        raw_gen = sheet_to_data(sheet)
        if raw_gen:

            xls_name = decode_sys( os.path.basename(xls_path) )
            desc = xls_name + '_' + sheet.name  # xlsName_sheetName

            lua_list.append(sheet_to_lua(sheet.name, raw_gen, desc, out_path))
            sheet2path[sheet.name] = decode_sys(xls_path)
        # except Exception as e:
        #     print >> sys.stderr.write(
        #         'convert FAILED ' + xls_path + ' ' + sheet2path.get(sheet.name) + '\n')
        #     raise

    return lua_list


def convert_xls(xls_name, out_path, sheet2path):
    xls_path = join(PATH_CFG, xls_name)
    lua_paths = excute_convert(xls_path, out_path, sheet2path)

    xls_name = decode_sys(xls_name)
    lua_list = xls2record_new[xls_name][key_lua]
    for lua_path in lua_paths:
        lua_list[lua_path] = file_util.file_md5(lua_path)

    return


# --//-------∽-★-∽------∽-★-∽--------∽-★-∽dirty∽-★-∽--------∽-★-∽------∽-★-∽--------//

xls2record = {}
xls2record_new = {}

dirty_list = []
fold2dirtylist = {}

key_hash = 'md5'
key_lua = 'lua_list'


def add_xls_dirty(xls_name):
    dirty_list.append(xls_name)
    xls2record_new[xls_name] = {key_lua: {}}


# name 道具表.xls | chenshi/道具表.xls
def check_xls_dirty(name):
    xls_path = join(PATH_CFG, name)
    md5 = file_util.file_md5(xls_path)

    xls_name = decode_sys(name)  # 转unicode,json打包gbk会报错

    is_dirty = False

    if (not xls2record.has_key(xls_name)) or xls2record[xls_name].get(key_hash) != md5:
        print('1-> will convert [%s]' % xls_name)
        add_xls_dirty(xls_name)
        is_dirty = True
    else:
        record = xls2record[xls_name]
        if record.has_key(key_lua):
            lua2md = xls2record[xls_name][key_lua]
            for k, v in lua2md.items():
                lua_path = k
                if not os.path.exists(lua_path):
                    print('2-> will convert [%s]' % xls_name)
                    add_xls_dirty(xls_name)
                    is_dirty = True
                else:
                    lua_md5 = file_util.file_md5(lua_path)
                    if lua_md5 != v:
                        print('3-> will convert [%s]' % xls_name)
                        add_xls_dirty(xls_name)
                        is_dirty = True

        else:
            print('4-> will convert [%s]' % xls_name)
            add_xls_dirty(xls_name)
            is_dirty = True

    if is_dirty:
        xls2record_new[xls_name][key_hash] = md5
    else:
        xls2record_new[xls_name] = xls2record[xls_name]
        print('skip xls [%s]' % xls_name)

    return is_dirty


# --//-------∽-★-∽------∽-★-∽--------∽-★-∽enumerate∽-★-∽--------∽-★-∽------∽-★-∽--------//


# 解析枚举xls
# 一般用sheet的name列作枚举值，没有name的话会用id列作枚举值，使用时用name，会转为id导出
def convert_enumerate(xls_path, sub_names, out_path):
    assertx(os.path.isfile(xls_path), xls_path + ' is required')

    with xlrd.open_workbook(xls_path, on_demand=True) as workbook:
        assertx(workbook.nsheets == 1, 'only one sheet for enum')
        sheet = workbook.sheet_by_index(0)

    name2items = {}

    raw_gen = sheet_to_data(sheet)
    for col, row_data in enumerate(raw_gen):
        enum_name = row_data['enum_name']
        xls_name = row_data['file_name']
        sheet_name = row_data['sheet_name']  # 枚举所在的页签
        no_name = int(row_data['no_name'])  # 不使用name作枚举
        base_name = xls_name[:-4]  # 去掉后缀，用来模糊搜索分区表str

        enum_name = decode_std(enum_name)  # 转unicode
        xls_name = decode_std(xls_name)
        sheet_name = decode_std(sheet_name)
        # base_name = decode_std(base_name) # 这个不转

        item = {'enum_name': enum_name,
                'xls_name': xls_name,
                'sheet_name': sheet_name,
                'no_name': no_name,
                'base_name': base_name}

        if enum_name not in name2items:
            items = []
            name2items[enum_name] = items
        else:
            items = name2items[enum_name]
        items.append(item)

    for enum_name, items in name2items.items():

        is_dirty = False
        for item in items:
            xls_name = item['xls_name']
            base_name = item['base_name']

            is_dirty = xls_name in dirty_list
            if not is_dirty:
                for sub_name in sub_names:  # 查找所有分区表名
                    # if 'G怪物' in base_name:
                    #     a = 1
                    u_base_name = decode_std(base_name)
                    u_sub_name = decode_sys(sub_name)  # dirty_list是unicode
                    if u_base_name in u_sub_name:
                        if u_sub_name in dirty_list:
                            is_dirty = True
                            break

            if is_dirty:
                break

        if not is_dirty:
            print('skip enumerate: %s' % enum_name)
            enum_tbl[enum_name] = 0  # 跳过，标记0
            continue

        print('converting enumerate: %s' % enum_name)

        for item in items:
            xls_name = item['xls_name']
            base_name = item['base_name']
            sheet_name = item['sheet_name']
            no_name = item['no_name']

            paths = [join(PATH_CFG, encode_std2sys(xls_name))]
            for sub_name in sub_names:  # 查找所有分区表名
                u_base_name = decode_std(base_name)
                u_sub_name = decode_sys(sub_name)
                if u_base_name in u_sub_name:
                    paths.append(join(PATH_CFG, sub_name))

            for index in range(0, len(paths)):
                path = paths[index]
                # path = encode_std2sys(path)

                print('\t-> begin process xls ' + decode_sys(path))
                assertx(os.path.isfile(path), path + ' is required')

                with xlrd.open_workbook(path, on_demand=True) as workbook:
                    has_sheet = False
                    for s in workbook.sheet_names():
                        if s == sheet_name or s.startswith(sheet_name + "_"):  # 天赋技能/天赋技能_XXX
                            sheet = workbook.sheet_by_name(s)
                            has_sheet = True
                            break
                    if not has_sheet:
                        continue  # 找不到页签，找下一个path

                name_row = sheet.row_values(1)  # 第二行：变量名
                id_col = name_row.index('id')  # 'id'所在的列
                id_type = sheet.cell_value(0, id_col)  # 'id'的类型
                if no_name == 0:  # 需要有名字
                    name_col = name_row.index('name')

                name2idValue = {}
                idValueSet = {}

                name2idValueOld = enum_tbl.get(enum_name)  # 之前已经生成过
                if name2idValueOld:
                    idValueSetOld = set(name2idValueOld.itervalues())  # 值列表转Set

                for row in range(ROW_VAL, sheet.nrows):
                    id_value = sheet.cell_value(row, id_col)
                    if id_value == '':
                        break  # 没有值了，跳出
                    if no_name == 0:
                        name_value = sheet.cell_value(row, name_col)  # 拿到的是unicode
                        name_value = str(name_value)  # test
                    else:
                        name_value = str(int(id_value))

                    assertx(id_value not in idValueSet, '枚举名ID重复[%s]' % id_value)
                    assertx(name_value not in name2idValue, '枚举名重复[%s]' % name_value)

                    if name2idValueOld:
                        assertx(id_value not in idValueSetOld, '与其他同类型表 枚举名ID重复[%s]' % id_value)
                        assertx(name_value not in name2idValueOld, '与其他同类型表 枚举名重复[%s]' % name_value)

                    try:
                        name_value = int(name_value)  # 这是为什么
                        name_value = unicode(name_value)
                    except:
                        pass

                    id_type = id_type.split('@')[0]
                    name2idValue[name_value] = cell_to_value(id_type, id_value)
                    idValueSet[id_value] = True

                if name2idValueOld:
                    name2idValueOld.update(name2idValue)
                    name2idValue = name2idValueOld
                enum_tbl[enum_name] = name2idValue

                desc = xls_name + '_' + sheet_name
                enum_to_lua(enum_name, name2idValue, desc, out_path)


# end def

# --//-------∽-★-∽------∽-★-∽--------∽-★-∽main∽-★-∽--------∽-★-∽------∽-★-∽--------//

global PATH_CFG


def main():
    global PATH_CFG

    PATH_ROOT = PATH_RUN[0:PATH_RUN.find("xls2lua")] + "xls2lua"
    PATH_CFG = PATH_RUN[0:PATH_RUN.find("xls2lua")] + "xls"  # 配置表目录

    if is_client:
        OUT_PATH_LUA = join(PATH_ROOT, "tmp/lua_data_raw_c")  # rawdata输出目录   xls2lua/lua_data_raw_c
        OUT_PATH_ENUM = join(PATH_ROOT, "tmp/lua_data_enum_c")
        OUT_PATCH_RECORD = join(PATH_ROOT, "tmp")
    else:
        OUT_PATH_LUA = join(PATH_ROOT, "tmp/lua_data_raw_s")
        OUT_PATH_ENUM = join(PATH_ROOT, "tmp/lua_data_enum_s")
        OUT_PATCH_RECORD = join(PATH_ROOT, "tmp")

    print("cfg path is: %s" % PATH_CFG)
    print("root path is: %s" % PATH_ROOT)
    print("lua out path is: %s" % OUT_PATH_LUA)

    PATH_record_xls = join(OUT_PATCH_RECORD, 'record_xls.json')

    if use_skip:
        if os.path.exists(PATH_record_xls):
            try:
                f = open(PATH_record_xls, 'r')
                global xls2record
                xls2record = json.loads(f.read(), strict=False)
                f.close()
            except:
                pass
    else:
        file_util.remove_dir(OUT_PATH_LUA)

    file_util.ensure_folder_exists(OUT_PATH_ENUM)
    file_util.ensure_folder_exists(OUT_PATH_LUA)
    file_util.ensure_folder_exists(OUT_PATCH_RECORD)

    all_paths = []
    sub_names = []
    fold2dirtylist[''] = []

    for name in os.listdir(PATH_CFG):
        path = join(PATH_CFG, name)
        if is_xls_file(path):
            all_paths.append(path)
            if check_xls_dirty(name):
                fold2dirtylist[''].append(name)
        else:
            # 分区资源表
            t_name = name + "/"
            fold2dirtylist[t_name] = []
            for sub_name in os.listdir(path):
                file_name = name + "/" + sub_name  # '文件夹/表名'
                sub_path = join(PATH_CFG, file_name)
                if is_xls_file(sub_path):
                    all_paths.append(sub_path)
                    sub_names.append(file_name)
                    if check_xls_dirty(file_name):
                        fold2dirtylist[t_name].append(sub_name)

    print('')

    # 先解析枚举
    enum_path = join(PATH_CFG, "enumerate.xls")
    convert_enumerate(enum_path, sub_names, OUT_PATH_ENUM)

    print('')

    for d, names in fold2dirtylist.items():
        if len(names) > 0 and len(d) > 0:
            print(decode_std('分区表资源:') + d)
        for name in names:
            sheet2path = {}
            convert_xls(join(d, name), join(OUT_PATH_LUA, d), sheet2path)
        if len(names) > 0:
            print('-' * 20)

    open(PATH_record_xls, 'wb').write(json.dumps(xls2record_new))

    print('')
    print(decode_std('资源处理完毕!!!'))
    return


if __name__ == "__main__":
    main()
