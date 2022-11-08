with open('popular.txt') as f:
    allwords = f.read().splitlines() 

fiveletterwords = []

for x in allwords:
    if len(x) == 5:
        fiveletterwords.append(x)

print(fiveletterwords)

with open('easy_five_letter_dictionary.txt', 'w') as f:
    for word in fiveletterwords:
        f.write(word+"\n")