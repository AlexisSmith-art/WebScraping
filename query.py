import sqlite3
import sys

# The main program. Returns True if the url is in the database, else inserts url into database then returns False.
def main():
    try:
        create_table()
        url = (sys.argv[1],)
        urls = select_unsent()
        if url in urls:
            print("Already in database!")
            return True
        else:
            print("Not in database!")
            response = insert_unsent(url[0])
            return False
    except IndexError:
        raise Exception("You need a string for this to work!")
    except KeyError:
        raise Exception("Not sure how you can get this error, but if you do, let me know!")
    except TypeError:
        raise Exception("Not sure how you can get this error, but if you do, let me know!")

# Creates the table if it does not already exist in the database.
def create_table():
    query = '''CREATE TABLE IF NOT EXISTS "urls" (
	    "id"	INTEGER NOT NULL UNIQUE,
	    "url"	TEXT,
	    PRIMARY KEY("id" AUTOINCREMENT)
        );'''
    result = open_and_close_db(query)
    return result

# Deletes all rows from the urls table.
def delete_all():
    query = "DELETE FROM urls"
    result = open_and_close_db(query)
    return result

# Inserts the url into the table.
def insert_unsent(url):
    query = "INSERT INTO urls (url) VALUES (?)"
    parameters = (url,)
    result = open_and_close_db(query, parameters)
    return result

# Retrieves a list of tuples with each tuple containing the urls
def select_unsent():
    query = "SELECT url FROM urls"
    result = open_and_close_db(query)
    return result

# Takes a query and runs it. The parameters argument fills in for the ? in the query.
def open_and_close_db(query, parameters=None):
    conn = sqlite3.connect("craiglist.db")
    c = conn.cursor()
    if parameters:
        c.execute(query, (parameters))
    else:
        c.execute(query)
    result = c.fetchall()
    conn.commit()
    conn.close()
    return result

# Runs the program
main()
