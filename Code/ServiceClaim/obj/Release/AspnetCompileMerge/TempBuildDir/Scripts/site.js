$(document).ready(function () {
    //editMulticolSelects();

});

function editMulticolSelects() {
    var s = document.getElementsByTagName('SELECT')[0].options,
       l = 0,
       d = '';
    for (i = 0; i < s.length; i++) {
        if (s[i].text.length > l) l = s[i].text.length;
    }
    var maxLength=0;
    for (i = 0; i < s.length; i++) {
        line = s[i].text.split(';');
        l1 = line[0].length;
        if (l1 > maxLength) maxLength = l1;
    }
    
    for (i = 0; i < s.length; i++) {
        d = '';
        line = s[i].text.split(';');
        l1 = (l - line[0].length);
        //alert(line[0].length);
        for (j = 0; j < maxLength+3 - line[0].length; j++) {
            d += '\u00a0';
        }
        s[i].text = line[0] + d + line[1];
    }
}

$(document).on('submit', 'form', function () {
    var button = $(this).find("[type='submit']");
    setTimeout(function () {
        button.attr('disabled', 'disabled');
    }, 0);

    //var button2 = $(this).find("button");
    //setTimeout(function () {
    //    button2.attr('disabled', 'disabled');
    //}, 0);
});