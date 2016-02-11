import sqlite3
from datetime import datetime

databasename = "sms.db"
filename = "messages.html"
cssfile = "style.css"
phonenumber = ""
	
def writeRow(file, row):
	id = row[0]
	message = row[1]
	utcdate = row[2]	
	name = ''
    
	if id == 3:
		
		name = 'Person One'
		trclass = 'person_one'
		tdclass = 'name1'		
	else:
		
		name = 'Person Two'
		trclass = 'person_two'
		tdclass = 'name2'
	
	
	date = datetime.fromtimestamp(utcdate)
	
	file.write('\t\t\t<tr class=' + trclass +'>\n')
	file.write('\t\t\t\t<td class=' + tdclass +' height=17 align=left>' + str(name) + '</td>\n')
	file.write('\t\t\t\t<td align=center>' + str(date) + '</td>\n')
	if message != None:
		file.write('\t\t\t\t<td align=left>' + message.encode('utf8') + '</td>\n')
	else:
		file.write('\t\t\t\t<td align=left></td>\n')
	file.write('\t\t\t</tr>\n')			
		
def writeCSS(file):
	f = open(cssfile, "r")	
	file.write(f.read())
	f.close()
	
def writeHeader(file):
	file.write('<html>\n')
	file.write('<head>\n')
	writeCSS(file)
	file.write('</head>\n')
	file.write('<body text="#000000">\n')
	file.write('\t<table frame=void cellspacing=0 cols=3 rules=none border=0>\n')
	file.write('\t\t<colgroup><col width="12%"><col width="12%"><col width="76%"></colgroup>\n')
	file.write('\t\t<tbody>\n')
	
def writeFooter(file):
	file.write('\t\t</tbody>\n')
	file.write('\t</table>\n')
	file.write('</body>\n')
	file.write('</html>\n')
		

print "Start"
conn = sqlite3.connect(databasename)
print "Connection Open"
c = conn.cursor()
c.execute("select flags, text, date from message where address='+" + phonenumber + "' order by date")
print "Statement Executed"
	
file = open(filename, "w")
print "File Opened"
	
writeHeader(file)
	
for row in c:
	writeRow(file, row)		
		
writeFooter(file)
file.close()	
print "File Closed"