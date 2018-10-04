# __author__ = jr.zeng
# -*- coding:utf-8 -*


class Const(object):

    class ConstError(TypeError):
        pass

    def __init__(self, dic=None):
        if dic:
            for k in dic:
                self[k] = dic[k]

    def __setattr__(self, key, value):
        if self.__dict__.has_key(key):
            raise self.ConstError, "Changing const.%s" % key
        else:
            self.__dict__[key] = value

    def __setitem__(self, key, value):
        if self.__dict__.has_key(key):
            raise self.ConstError, "Changing const.%s" % key
        else:
            self.__dict__[key] = value

    def __getattr__(self, key):
        if self.__dict__.has_key(key):
            return self.key
        else:
            return None
