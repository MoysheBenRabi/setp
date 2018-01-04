from datetime import datetime
from time import mktime
epoch_2k = datetime(2000, 01, 01)

def mkts(value):
    return int((mktime(value.timetuple()) + \
           value.microsecond / 1e6)*1e3)

epoch_2kts = mkts(epoch_2k)
