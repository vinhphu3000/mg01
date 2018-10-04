# __author__ = jr.zeng
# -*- coding:utf-8 -*

__all__ = ['string_util',
           'list_util',
           'file_util',
           'const',
           'assertx',
           'stdout_encoding',
           'preferred_encoding',
           'encode_std2sys',
           'decode_sys',
           'decode_std',
           ]

import sys
import chardet
import list_util

from locale import getpreferredencoding

# 系统编码
preferred_encoding = getpreferredencoding()
preferred_encoding = preferred_encoding.lower()
print('check preferred_encoding', preferred_encoding)

# 当前输出编码
stdout_encoding = sys.stdout.encoding
#stdout_encoding = sys.getdefaultencoding()
if not stdout_encoding or stdout_encoding == 'US-ASCII' or stdout_encoding == 'ascii':
    stdout_encoding = 'utf-8'

stdout_encoding = stdout_encoding.lower()
print('check stdout_encoding', stdout_encoding)

if sys.stderr.encoding == 'cp936':
    class UnicodeStreamFilter(object):
        def __init__(self, target):
            self.target = target
            self.encoding = 'utf-8'
            self.errors = 'replace'
            self.encode_to = self.target.encoding

        def write(self, s):
            if isinstance(s, bytes):
                s = s.decode('utf-8')
                # print("UnicodeStreamFilter", s, self.encode_to, self.errors)
            s = s.encode(self.encode_to, self.errors).decode(self.encode_to)
            self.target.write(s)


    sys.stderr = UnicodeStreamFilter(sys.stderr)


def assertx(value, message):
    if __debug__ and not value:
        try:
            code = compile('1 / 0', '', 'eval')
            exec (code)
        except ZeroDivisionError:
            tb = sys.exc_info()[2].tb_next
            assert tb
            # raise AssertionError, str(message).encode('utf-8'), tb
            raise AssertionError, str(message), tb
    return value


# 系统编码->unicode
def decode_sys(value):
    return value.decode(preferred_encoding)


# 标准编码->unicode
def decode_std(value):
    return value.decode('utf-8')



# 系统编码->输出编码
# def encode_sys2out(value):
#     if stdout_encoding == 'utf-8':  # ide环境
#         return value.decode(preferred_encoding).encode(stdout_encoding)
#     else:
#         return value


# 标准编码->输出编码
# def encode_std2out(value):
#     if stdout_encoding == 'utf-8':  # ide环境
#         return value
#     else:
#         return value.decode('utf-8').encode(preferred_encoding)


# 标准编码->系统编码
def encode_std2sys(value):
    return value.decode('utf-8').encode(preferred_encoding)  # 转为系统编码

# def encode_sys2std(value):
#     return value.decode(preferred_encoding).encode(stdout_encoding)
