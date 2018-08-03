import os
index = ''
while True:
    try:
        os.makedirs('../hi'+index)
        break
    except WindowsError:
        if index:
            index = '('+str(int(index[1:-1])+1)+')' # Append 1 to number in brackets
        else:
            index = '(1)'
        pass # Go and try create file again
    # print(index)