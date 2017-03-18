jQuery(function () {
    $("#myEditor").val(c.HtmlDecode(c.HtmlDecode(content)));
    var editor = new UE.ui.Editor({ initialFrameHeight: 400, initialFrameWidth: "auto" });
    editor.render("myEditor");

    $("button[name='save']").click(function () {
        var message = UE.getEditor('myEditor').getContent();
        $("input[name='save_content']").val(c.HtmlEncode(message));
    });
   
});