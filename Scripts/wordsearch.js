var wordlist = document.getElementById('WordListEntered');

function checkUserEntry() {   
    var wordsizelimit = document.getElementById('WordSearchGridSize').value;
    var gridsizelen = wordsizelimit * wordsizelimit;
    var entry = document.getElementById('WordListEntry').value;
    var validentry = [];
    var invalidentry = "";
    var entryArray = entry.split(' ');
    for (i = 0; i < entryArray.length; ++i) {
        if (entryArray[i].length <= wordsizelimit && isLetter(entryArray[i])) {
            validentry.push((entryArray[i]));
        } else {
            invalidentry = invalidentry + " " + entryArray[i];
        }
    }
    enterValidWords(validentry);
    populateEntryInvalid(invalidentry);
};

function enterValidWords(validentry) {
    for (i = 0; i < validentry.length; i++) {
        var wordArray = document.createElement('option');
        wordArray.text = validentry[i];
        wordlist.add(wordArray);
    }
}

function populateEntryInvalid(invalidentry) {
    document.getElementById("WordListEntry").value = invalidentry;
}

//Check if the string is all valid letters.
//Solution found at: https://stackoverflow.com/questions/4434076/best-way-to-alphanumeric-check-in-javascript
function isLetter(str) {
    return str.match(/^[a-z0-9]+$/i);
}

//If the user hits shift+enter, populate the list.
$("#WordListEntry").keypress(function (e) {
    if (e.keyCode == 13 && e.shiftKey) {
        checkUserEntry();
        e.preventDefault();
    }
});

