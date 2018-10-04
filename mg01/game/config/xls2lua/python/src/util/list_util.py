# __author__ = jr.zeng
# -*- coding:utf-8 -*


#获取指定下标的元素
def get(list, index):
    if len(list) > index:
        return list[index]
    return None