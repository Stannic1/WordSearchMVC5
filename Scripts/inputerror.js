//I wanted to test if this worked and it actually did.
$(document).ready(function () {
    alert("Error: Too many words or words are too big.");
    $.ajax({
        type: 'POST',
        url: '/Home',
        success: function (res) {
            window.location.href = '/Home';
        }
    });
})