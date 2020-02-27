function checkUserEntry() {   
    var wordsizelimit = document.getElementById('numbers').value;
    var gridsizelen = wordsizelimit * wordsizelimit;
    var entry = document.getElementById('words').value;
    var entryArray = entry.split(' ');
    var errormsg = "";
    for (i = 0; i < entryArray.length; ++i) {
        if (entryArray[i].length > wordsizelimit && isLetter(entryArray[i])) {
            errormsg = entryArray[i] + " is too long or has an invalid letter!";
            break;
        }

        gridsizelen -= entryArray[i].length;
        if (gridsizelen < 0) {
            errormsg = "Amount of words is too big!";
        }
    }
    alert(errormsg);
};
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

failed = function (xhr) {
    checkUserEntry();
}

success = function (xhr) {
    console.log({ xhr });
}