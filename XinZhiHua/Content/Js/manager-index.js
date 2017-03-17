jQuery(function () {
   
    var InitUpload = function (box) {
        var type = box.find("input[name='type']").val();
        var upload_show_img = box.find("div[name='upload_show_img']");

       var $ = jQuery,
       // 优化retina, 在retina下这个值是2
       ratio = window.devicePixelRatio || 1,
       // 缩略图大小
       thumbnailWidth = 200 * ratio,
       thumbnailHeight = 135 * ratio,
       // Web Uploader实例
       uploader;

        // 初始化Web Uploader
        uploader = WebUploader.create({

            // 自动上传。
            auto: false,

            // swf文件路径
            swf: '/Content/Lib/webuploader-0.1.5/Uploader.swf',

            // 文件接收服务端。
            server: '/manager/UploadImg?key=' + type,

            // 选择文件的按钮。可选。
            // 内部根据当前运行是创建，可能是input元素，也可能是flash.
            pick: 'div[name="upload_img_' + type + '"]',

            // 只允许选择文件，可选。
            accept: { // 只允许选择图片文件。
                extensions: 'gif,jpg,jpeg,png',
                mimeTypes: 'image/gif,image/jpeg,image/png'
            }
        });

        // 当有文件添加进来的时候
        uploader.on('fileQueued', function (file) {
            upload_show_img.empty();
            var $li = $(
                    '<div id="' + file.id + '" class="file-item thumbnail">' +
                        '<img>' +
                        '<div class="info">' + file.name + '</div>' +
                    '</div>'
                    ),
                $img = $li.find('img');
            upload_show_img.append($li);

            // 创建缩略图
            uploader.makeThumb(file, function (error, src) {
                if (error) {
                    $img.replaceWith('<span>不能预览</span>');
                    return;
                }
                $img.attr('src', src);
            }, thumbnailWidth, thumbnailHeight);
            box.find("div[name='upload_show_img']").css("display","block");
        });

        // 文件上传成功，给item添加成功class, 用样式标记上传成功。
        uploader.on('uploadSuccess', function (file) {
            location.reload();
        });

        // 文件上传失败，现实上传出错。
        uploader.on('uploadError', function (file) {
            var $li = $('#' + file.id),
                $error = $li.find('div.error');
            // 避免重复创建
            if (!$error.length) {
                $error = $('<div class="error"></div>').appendTo($li);
            }
            $error.text('上传失败');
        });
        return uploader;

    }

    var UpdateContent = function (box) {
        var content = box.find("textarea[name='content']").val();
        var type = box.find("input[name='type']").val();
        $.ajax({
            cache: false,
            url: "/manager/IndexPost",
            type: "post",
            async:true,
            data: {
                content: content,
                type: type
            },
            dataType: "json",
            success: function (json) {
                if (json) {
                    box.find("span[name='error']").text("保存文本成功");
                } else {
                    box.find("span[name='error']").text("保存文本失败");
                }
            }, error: function () {
                box.find("sapn[name='error']").text("保存文本失败");
            }
        });
    }

    $.each($("[name='box']"), function () {
        var box = $(this);
        var uploader = InitUpload(box);

        box.find("button[name='sub']").click(function () {
            UpdateContent(box);
            uploader.upload();
        });
    });

});