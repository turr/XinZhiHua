var c = {
    HtmlEncode: function (value) {
        return $('<div/>').text(value).html();
    },
    HtmlDecode: function (value) {
        return $('<div/>').html(value).text();
    }
}