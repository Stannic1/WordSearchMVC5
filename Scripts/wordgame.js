
$(function () {
    var selected = [];
    var selectcount = 0;
    var startx, starty;
    var dx, dy;
    var isMouseDown = false, isHighlighted;
    var validDirections = [];

    function sendToServer() {
        var sessionKey = document.getElementById("sessionkey").textContent;
        var makeString = "";
        for (var i = 0; i < selected.length; i++) {
            makeString = makeString + $(selected[i]).text();
        }
        
        console.log({ makeString });
        return $.ajax({
            type: 'POST',
            url: '/Home/WordSearchUserFind/',
            data: { input: makeString, key: sessionKey },
            datatype: "json",
            async: false
        });
    }

    function highlightCorrect() {
        for (var i = 0; i < selected.length; i++) {
            $(selected[i]).addClass("correct");
            var item = selected[i];
            console.log({ item });
        }
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
    function canHighlight(x, y) {
        for (var i = 0; i < validDirections.length; i += 2) {
            if (x == validDirections[i] && y == validDirections[i + 1])
                return true;
        }
        return false;
    }

    //Helper functions
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
    function createValidCells(x, y, dx, dy) {
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
                    for (var i = selected.length - 1; i > 0; i--) {
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
                            removecell = selected.pop(selected[selectcount - 1]);
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
        //Because Ajax is asynchronous, it is pretty hard to 
        //try and get something out of it. Learned it out the 
        //hard way and this is one of the solutions to this issue.
        //The other is using derived.
        sendToServer().done(function (data) {
            if (data.success == true) {
                highlightCorrect();
            }
            console.log({ data });
        })
        isMouseDown = false;
        deselectAll();
    });

});
