﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public static class NameGenerator
{
    public static string GenName()
    {
        string sal = salutation[Random.Range(0, salutation.Length - 1)];
        string first = (Random.Range(0, 1) == 0) ? maleNames[Random.Range(0, maleNames.Length - 1)] : maleNames[Random.Range(0, femaleNames.Length - 1)];
        string last = surnames[Random.Range(0, surnames.Length - 1)];

        return sal + " " + first + " " + last;
    }

    public static string GenContent()
    {
        return domain[Random.Range(0, domain.Length - 1)] + actions[Random.Range(0, actions.Length - 1)];
    }

    static string[] domain = {
        "youtube.com",
        "amazon.com",
        "google.com",
        "pornhub.com",
        "imgur.com",
        "reddit.com",
        "mslive.com",
        "github.com",
        "vimeo.com",
        "rbc.com",
        "cibc.com",
        "yahoo.com",
        "meatspin.com",
        "aol.net"
    };

    static string[] actions ={
        "?search=cats",
        "?search=how_to_get_rich",
        "?search=paris_hilton",
        "?search=game_jarm",
        "?search=doggo_memes",
        "?search=how_make_baby",
        "?search=bitcoin_daddies",
        "?search=french_toast",
        "/login?user=meatSpin420",
        "/login?user=pirateJim69",
        "/login?user=ballerDad",
        "/login?user=iHateSnapchat",
        "/login?user=kittens4ever",
        "/login?user=notABot",
        "/dogs",
        "/geese",
        "/canada",
        "/delete_my_account",
        "/checkout"
    };

    static string[] salutation =  {
        "Grampa",
        "Gramma",
        "Little",
        "Mr.",
        "Mrs.",
        "Miss",
        "Dr.",
        "Lt.",
        "Private.",
        "Officer",
        "Janitor",
        "Bot",
    };

    static string[] maleNames = {
        "Joshua",
        "Jerry",
        "Dennis",
        "Walter",
        "Patrick",
        "Peter",
        "Harold",
        "Douglas",
        "Henry",
        "Carl",
        "Arthur",
        "Ryan",
        "Roger",
        "Joe",
        "Juan",
        "Jack",
        "Albert",
        "Jonathan",
        "Justin",
        "Terry",
        "Gerald",
        "Keith",
        "Samuel",
        "Willie",
        "Ralph",
        "Lawrence",
        "Nicholas",
        "Roy",
        "Benjamin",
        "Bruce",
        "Brandon",
        "Adam",
        "Harry",
        "Fred",
        "Wayne",
        "Billy",
        "Steve",
        "Louis",
        "Jeremy",
        "Aaron",
        "Randy",
        "Howard",
        "Eugene",
        "Carlos",
        "Russell",
        "Bobby",
        "Victor",
        "Martin",
        "Ernest",
        "Phillip",
        "Todd",
        "Jesse",
        "Craig",
        "Alan",
        "Shawn",
        "Clarence",
        "Sean",
        "Philip",
        "Chris",
        "Johnny",
        "Earl",
        "Jimmy",
        "Antonio",
        "Danny",
        "Bryan",
        "Tony",
        "Luis",
        "Mike",
        "Stanley",
        "Leonard",
        "Nathan",
        "Dale",
        "Manuel",
        "Rodney",
        "Curtis",
        "Norman",
        "Allen",
        "Marvin",
        "Vincent",
        "Glenn",
        "Jeffery",
        "Travis",
        "Jeff",
        "Chad",
        "Jacob",
        "Lee",
        "Melvin",
        "Alfred",
        "Kyle",
        "Francis",
        "Bradley",
        "Jesus",
        "Herbert",
        "Frederick",
        "Ray",
        "Joel",
        "Edwin",
        "Don",
        "Eddie",
        "Ricky",
        "Troy",
        "Randall",
        "Barry",
        "Alexander",
        "Bernard",
        "Mario",
        "Leroy",
        "Francisco",
        "Marcus",
        "Micheal",
        "Theodore",
        "Clifford",
        "Miguel"
    };

    static string[] femaleNames = {
        "Linda",
        "Barbara",
        "Elizabeth",
        "Jennifer",
        "Maria",
        "Susan",
        "Margaret",
        "Dorothy",
        "Lisa",
        "Nancy",
        "Karen",
        "Betty",
        "Helen",
        "Sandra",
        "Donna",
        "Carol",
        "Ruth",
        "Sharon",
        "Michelle",
        "Laura",
        "Sarah",
        "Kimberly",
        "Deborah",
        "Jessica",
        "Shirley",
        "Cynthia",
        "Angela",
        "Melissa",
        "Brenda",
        "Amy",
        "Anna",
        "Rebecca",
        "Virginia",
        "Kathleen",
        "Pamela",
        "Martha",
        "Debra",
        "Amanda",
        "Stephanie",
        "Carolyn",
        "Christine",
        "Marie",
        "Janet",
        "Catherine",
        "Frances",
        "Ann",
        "Joyce",
        "Diane",
        "Alice",
        "Julie",
        "Heather",
        "Teresa",
        "Doris",
        "Gloria",
        "Evelyn",
        "Jean",
        "Cheryl",
        "Mildred",
        "Katherine",
        "Joan",
        "Ashley",
        "Judith",
        "Rose",
        "Janice",
        "Kelly",
        "Nicole",
        "Judy",
        "Christina",
        "Kathy",
        "Theresa",
        "Beverly",
        "Denise",
        "Tammy",
        "Irene",
        "Jane",
        "Lori",
        "Rachel",
        "Marilyn",
        "Andrea",
        "Kathryn",
        "Louise",
        "Sara",
        "Anne",
        "Jacqueline",
        "Wanda",
        "Bonnie",
        "Julia",
        "Ruby",
        "Lois",
        "Tina",
        "Phyllis",
        "Norma",
        "Paula",
        "Diana",
        "Annie",
        "Lillian",
        "Emily",
        "Robin",
        "Peggy",
        "Crystal",
        "Gladys",
        "Rita",
        "Dawn",
        "Connie",
        "Florence",
        "Tracy",
        "Edna",
        "Tiffany",
        "Carmen",
        "Rosa",
        "Wendy",
        "Victoria",
        "Kim"
    };

    static string[] surnames = {
        "Wilson",
        "Moore",
        "Taylor",
        "Anderson",
        "Thomas",
        "Jackson",
        "White",
        "Harris",
        "Martin",
        "Thompson",
        "Garcia",
        "Martinez",
        "Robinson",
        "Clark",
        "Rodriguez",
        "Lewis",
        "Lee",
        "Walker",
        "Hall",
        "Allen",
        "Young",
        "Hernandez",
        "King",
        "Wright",
        "Lopez",
        "Hill",
        "Scott",
        "Green",
        "Adams",
        "Baker",
        "Gonzalez",
        "Nelson",
        "Carter",
        "Mitchell",
        "Perez",
        "Roberts",
        "Turner",
        "Phillips",
        "Campbell",
        "Parker",
        "Evans",
        "Edwards",
        "Collins",
        "Stewart",
        "Sanchez",
        "Morris",
        "Rogers",
        "Reed",
        "Cook",
        "Morgan",
        "Bell",
        "Murphy",
        "Bailey",
        "Rivera",
        "Cooper",
        "Richardson",
        "Cox",
        "Howard",
        "Ward",
        "Torres",
        "Peterson",
        "Gray",
        "Ramirez",
        "James",
        "Watson",
        "Brooks",
        "Kelly",
        "Sanders",
        "Price",
        "Bennett",
        "Wood",
        "Barnes",
        "Ross",
        "Henderson",
        "Coleman",
        "Jenkins",
        "Perry",
        "Powell",
        "Long",
        "Patterson",
        "Hughes",
        "Flores",
        "Washington",
        "Butler",
        "Simmons",
        "Foster",
        "Gonzales",
        "Bryant",
        "Alexander",
        "Russell",
        "Griffin",
        "Diaz",
        "Hayes",
        "Myers",
        "Ford",
        "Hamilton",
        "Graham",
        "Sullivan",
        "Wallace",
        "Woods",
        "Cole",
        "West",
        "Jordan",
        "Owens",
        "Reynolds",
        "Fisher",
        "Ellis",
        "Harrison",
        "Gibson",
        "Mcdonald",
        "Cruz",
        "Marshall",
        "Ortiz"
    };
}