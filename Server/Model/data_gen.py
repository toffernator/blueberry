import random
from string import Template
import datetime
import sys
import json

tags = {
    'React': {'Framework', 'Web Development'},
    'Java': {'Object Oriented Programming'},
    'Javascript': {'Web Development'},
    'Flutter': {'Mobile Development'},
    'TypeScript': {'Web Development'},
    'HTML': {'Web Development'},
    'CSS': {'Web Development'},
    'Docker': {'DevOps'},
    'Kubernetes': {'DevOps'},
}

types = ['Article', 'Video', 'Podcast']


one_word_phrases = [Template('Introduction to $word'),
                    Template('Taking a closer look at $word'),
                    Template('$word Tutorial')]
two_word_phrases = [Template('Working with $word1 in $word2'),
                    Template('How to use $word1 in $word2'),
                    Template('Doing $word1 with $word2')]

one_word_description = [Template('$word is the changing the way programmers work. Are you ready for the change?'),
                        Template('$word is an upcoming technology. Learn it before anyone else!'),
                        Template('Start learning $word now! It will change everything.')]

two_word_description = [Template('$word1 makes $word2 even better! Have you mastered the combination?'),
                        Template('$word1 with $word2 is more powerful than ever! Start learning now!'),
                        Template('Take $word1 to the next level with $word2. This is how to get started!')]

def random_date():
    start_date = datetime.date(2000, 1, 1)
    end_date = datetime.date(2021, 12, 31)

    time_between_dates = end_date - start_date
    days_between_dates = time_between_dates.days
    random_number_of_days = random.randrange(days_between_dates)
    return start_date + datetime.timedelta(days=random_number_of_days)



def generate_one_word_material():
    phrase = random.choice(one_word_phrases)
    description = random.choice(one_word_description)
    type = random.choice(types)
    word = random.choice(list(tags))
    tag_list = tags[word]
    tag_list.add(word)

    material = {
        "title": phrase.substitute(word=word),
        "tags": list(tag_list),
        "date": f"{random_date()}",
        "description": description.substitute(word=word),
        "type": type
    }

    return material 


def generate_two_word_material():
    phrase = random.choice(two_word_phrases)
    description = random.choice(two_word_description)
    type = random.choice(types)
    word1 = random.choice(list(tags))
    word2 = random.choice(list(tags))
    while word1 == word2:
        word2 = random.choice(list(tags))

    tag_list = set().union(tags[word1], tags[word2])
    tag_list.add(word1)
    tag_list.add(word2)

    material = {
        "title": phrase.substitute(word1=word1, word2=word2),
        "tags": list(tag_list),
        "date": f"{random_date()}",
        "description": description.substitute(word1=word1, word2=word2),
        "type": type
    }

    return material


json_tags = set()
for k in tags.keys():
    json_tags.add(k)

for i in tags.values():
    json_tags.update(i)
    
json_materials = []

if len(sys.argv) == 2:
    for i in range(int(sys.argv[1])):
        json_materials.append(generate_one_word_material())
        json_materials.append(generate_two_word_material())
else:
    for i in range(10):
        json_materials.append(generate_one_word_material())
        json_materials.append(generate_two_word_material())

output = {
    "tags": list(json_tags),
    "materials": json_materials
}

print(json.dumps(output))
