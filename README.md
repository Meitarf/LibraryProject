# LibraryProject
C# UWP project using OOP for managing a library.
## Installation
Download and run LibraryProject.sln on Visual Studio.
## Users Login
**For manager (librarian) authorization:**
```
User Name: Librarian
Password: 1111
```
**For customer authorization:**
```
User Name: Customer
Password: 2222
```
![image](https://user-images.githubusercontent.com/62158246/210226107-e6967068-0dfb-4c9e-90cc-746bb09a4f21.png)

## Overview
The **"Models"** folder has 4 classes: **"LibraryItem"** (an abstract class), **"Book", "Journal"** (both inherit from the abstract class) and **"Genre"** (enum).
The **"Users"** folder contains the class **"User"** and **"UserLogin"**, which contains the user logic.
The **"Manager"** folder has **"ItemCollection"** which contains a list of library items.
I also created a folder called **"Exceptions"** to display different messages for different type of errors.

### Customer Page
The customer page includes sepcific permissions, such as:
* Renting an item, or returning a rented item (depends on the item's state)
* Loading (refreshing) library items list
* Searching items (by different parameters: Name, Author, Publisher, Genre)

![image](https://user-images.githubusercontent.com/62158246/210230694-0ab31136-4b05-49f6-b79c-e56f28421983.png)

### Librarian Page
The librarian page consists with the customer permissions, with addition of administration operations such as:
* Editing items
* Creating and adding new items
* Creating discounts and applying them on items using filters (by different parameters: Name, Author, Publisher, Genre)
* Displaying a report of rented items
* Saving the current list of library items (with the added, or without the deleted) into an XML file and loading it

![image](https://user-images.githubusercontent.com/62158246/210231200-29c145ae-5714-4dfd-9ceb-e2af876a506d.png)

