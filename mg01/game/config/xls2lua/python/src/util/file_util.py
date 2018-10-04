# __author__ = jr.zeng
# -*- coding:utf-8 -*


import os
import shutil
import hashlib

from const import Const

FILE_SLASH = "\\"
FILE_SLASH_REVERSE = "/"


# 文件后缀
class Suffix():
    def __init__(self):
        pass

    NONE = ""
    BYTES = ".bytes"
    XML = ".xml"
    TXT = ".txt"
    XLS = ".xls"
    # XLSX = ".xlsx"
    LUA = ".lua"


# --//-------∽-★-∽------∽-★-∽--------∽-★-∽文件操作∽-★-∽--------∽-★-∽------∽-★-∽--------//


# 路径是否存在
def is_file_exist(path):
    b = os.path.exists(path)
    return b


# 确保文件夹存在
def ensure_folder_exists(path):
    if is_file_exist(path):
        # 路径存在
        print "[ensure_folder_exists] already %s" % path
        return True

    path = format_path(path)

    index = path.find(".")
    if index != -1:
        index = path.rfind(FILE_SLASH)  # 逆向查找
        path = path[0:index]  # 去掉文件名

    if path.endswith(FILE_SLASH):
        path = path[0:-len(FILE_SLASH)]  # 去掉末尾的斜杠

    index = path.find(FILE_SLASH, 0)
    while index != -1:
        folder = path[0:index]
        if not is_file_exist(folder):
            os.mkdir(folder)
        index += len(FILE_SLASH)
        index = path.find(FILE_SLASH, index)

    os.mkdir(path)
    print "[ensure_folder_exists] %s" % path
    return


# 写入文件
def write_file(path, content):
    file_object = open(path, 'w')
    try:
        file_object.write(content)
    finally:
        file_object.close()
    return


# 获取目录下的子文件夹
def get_all_folders(path, recursive=False):
    current_files = os.listdir(path)
    all_folders = []
    for file_name in current_files:
        if file_name[0] == '.':
            # 隐藏文件
            continue
        full_file_name = os.path.join(path, file_name)
        if os.path.isdir(full_file_name):
            # 是文件夹
            all_folders.extend(full_file_name)
            if recursive:   #递归子文件夹
                next_folders = get_all_folders(full_file_name)
                all_folders.extend(next_folders)
    return all_folders


# 获取目录下的文件列表
def get_files_from_directory(path, recursive=False, suffix=None):
    current_files = os.listdir(path)
    all_files = []
    for file_name in current_files:
        if file_name[0] == '.':
            # 隐藏文件
            continue
        full_file_name = os.path.join(path, file_name)
        if os.path.isdir(full_file_name):
            # 是文件夹
            if recursive:   #递归子文件夹
                next_level_files = get_files_from_directory(full_file_name)
                all_files.extend(next_level_files)
        else:
            if suffix:
                _, ext = os.path.splitext(full_file_name)
                if ext != suffix:
                    continue
            all_files.append(full_file_name)

    return all_files


# 删除文件
def remove_file(path, remove_self=True):
    if not is_file_exist(path):
        return

    if os.path.isdir(path):
        # 是文件夹
        file_list = os.listdir(path)
        for f in file_list:
            file_path = os.path.join(path, f)
            if os.path.isfile(file_path):
                os.remove(file_path)
                print "[removeFile] file removed:%s" % file_path
            elif os.path.isdir(file_path):
                shutil.rmtree(file_path, True)
                print "[removeFile] folder removed:%s" % file_path

        if remove_self:  # 如果是文件夹的话，删除自己
            shutil.rmtree(path, True)
            print "[removeFile] folder removed:%s" % path
    else:
        os.remove(path)
        print "[removeFile] file removed:%s" % path
    return


def remove_dir(path):
    if not is_file_exist(path):
        return

    if os.path.isdir(path):
        # 是文件夹
        shutil.rmtree(path, True)
        print "[remove_dir] folder removed:%s" % path



def file_md5(path):
    with open(path, 'rb') as f:
        return hashlib.md5(f.read()).hexdigest()

# --//-------∽-★-∽------∽-★-∽--------∽-★-∽TOOL∽-★-∽--------∽-★-∽------∽-★-∽--------//

# 格式化路径
def format_path(path):
    return str(path).replace('/', '\\')


# 获取文件后缀
def get_file_suffix(path):
    return os.path.splitext(path)[1]


# 格式化文件名
def modify_file_name(path, suffix=None, extra=None):
    file_name = path
    file_suffix = ""
    index = path.find(".")
    if index != -1:
        file_name = path[0:index]
        file_suffix = path[index:]

    if extra:
        file_name = file_name + extra

    if suffix:
        i = suffix.find(".")
        if i != -1:
            file_name = file_name + suffix
        else:
            file_name = file_name + "." + suffix
    else:
        file_name = file_name + file_suffix
    return file_name


# 获取父级路径
# up_dep 往上的层数
def get_parent_path(path, up_dep=1):
    result = format_path(path)
    while up_dep > 0:
        up_dep -= 1
        idx = result.rfind(FILE_SLASH)
        if idx >= 0:
            result = result[0:idx]
    return result
