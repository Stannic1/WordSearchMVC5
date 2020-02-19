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


function mousePress() {

}

$(function () {
    var selected = [];
    var selectcount = 0;
    var startx, starty;
    var dx, dy;
    var isMouseDown = false, isHighlighted;
    var validDirections = [];

    function debugXY(x,y) {
        console.log('%c X and Y', 'color: teal', 'font-weight: bold;');
        console.log({ x, y });
    }
    //destroy selection
    function deselectAll() {
        for (var i = 0; i < selected.length; i++) {
            selected[i].removeClass("highlighted");
        }
        selected = [];
        validDirections = [];
    }
    //Checks to see if the grid can be highlighted.
    function canHighlight(x,y) {
        for (var i = 0; i < validDirections.length; i+=2) {
            if (x == validDirections[i] && y == validDirections[i + 1])
                return true;
        }
        return false;
    }

    //Functions to help make reading some of the lines easier.
    function isRowValid(i) {
        var rowcount = document.getElementById("selected_table").rows.length;
        return ((i >= 0) && (i <= rowcount));
    }

    function isColValid(j) {
        var rowcount = document.getElementById("selected_table").rows.length;
        return ((j >= 0) && (j <= rowcount));
    }

    function getColPos(col) {
        return $(col).parent().children().index($(col));
    }

    function getRowPos(row) {
        return $(row).parent().parent().children().index($(row).parent());
    }
    //Iterate through the direction based on startx, starty, dx, and dy
    //to create an array that the user can traverse through.
    function createValidCells(x, y, dx,dy) {
        var movetable = [];
        var i = x;
        var j = y;
        while (isRowValid(i) && isColValid(j)) {
            movetable.push(i + dx);
            movetable.push(j + dy);
            i += dx;
            j += dy;
        }
        return movetable;
    }

    $("#selected_table td").mousedown(function () {
        isMouseDown = true;
        isSelecting = true;
        selectcount = 0;
        $(this).toggleClass("highlighted");
        isHighlighted = $(this).hasClass("highlighted");
        startx = getRowPos($(this));
        starty = getColPos($(this));
        startPos = isHighlighted.selectionStart;
        selected.push($(this));
        selectcount++;
        return false;
    })
    .mouseover(function () {
        if (isMouseDown) {
            dx = getRowPos($(this));
            dy = getColPos($(this));
            //If the user is back at the start, deselect everything except 
            //the starting cell.
            if (startx == dx && starty == dy) {
                for (var i = selected.length-1; i > 0; i--) {
                    selected[i].removeClass("highlighted");
                }
                validDirections = [];
            } else if (validDirections.length == 0) { 
            //If the user decided to traverse in a direction, get the
            //direction by subtracting the start and new cell to get 
            //the direction such as (0, 1), (0, -1), and other ways around.
                var dirx = dx - startx;
                var diry = dy - starty;
                console.log({ dirx, diry });
            //then create a valid array that the user can pass through.
                validDirections = createValidCells(startx, starty, dirx, diry);
            }
            //If the element exists in the array, traverse through it.
            if (canHighlight(dx, dy)) {
                //if it is already highlighted, it assumes the user is going back.
                if ($(this).hasClass("highlighted")) {
                    while (dx != getRowPos(selected[selectcount - 1]) || dy != getColPos(selected[selectcount - 1])) {
                        console.log({ selectcount });
                        removecell = selected.pop(selected[selectcount-1]);
                        removecell.removeClass("highlighted");
                        selectcount--;
                    }
                } else {
                    selected.push($(this));
                    $(this).toggleClass("highlighted", isHighlighted);
                    selectcount++;
                }
            }
        }
    })
    .bind("selectstart", function () {
        return false;
    })  

    $(document).mouseup(function () {
        isMouseDown = false;
        deselectAll();
    });

});

