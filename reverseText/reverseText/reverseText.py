text = "this is some text"
textArray = []
length = len(text)

for count in range(length-1, -1, -1):
    textArray.append(text[count])

resultText = ''.join(textArray)
print(resultText)
print("hi")