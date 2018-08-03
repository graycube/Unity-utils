import triad_openvr
import time
import sys
import csv
import os
import serial

v = triad_openvr.triad_openvr()
v.print_discovered_objects()

index=''

ser = serial.Serial('COM8', 115200, timeout=0)
stagePos = '0'

if len(sys.argv) == 1:
    interval = 1/50
elif len(sys.argv) == 2:
    interval = 1/float(sys.argv[0])
else:
    print("Invalid number of arguments")
    interval = False

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

filename="%s.csv"%index        

with open(filename, 'w', newline='') as csvfile:
    wr = csv.writer(csvfile, delimiter=' ',
                    quoting=csv.QUOTE_MINIMAL)

if interval:
    while(True):
        start = time.time()
        #txt = ""
        txt = stagePos
        txt += " "
        posData=[]
        for each in v.devices["controller_1"].get_pose_euler():
            posData.append(each)
            posData.append("\t")
            txt += "%.4f" % each
            txt += " "
        print("\r" + txt, end="")
        
        with open(filename, 'a', newline='') as csvfile:
            wr = csv.writer(csvfile, delimiter=' ',
                    quoting=csv.QUOTE_MINIMAL)
            wr.writerow(posData)

        sleep_time = interval-(time.time()-start)
        if sleep_time>0:
            stagePos = "NULL"
            data = ser.readline()
            if data:
                stagePos = data[4::].split()[0]
            time.sleep(sleep_time)