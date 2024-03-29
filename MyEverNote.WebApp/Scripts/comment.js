﻿var noteid = -1;
var modalCommentBodyId = "#modal_comment_body";


$(function () {

    $('#modal_comment').on('show.bs.modal', function (e) {

        var btn = $(e.relatedTarget);
        noteid = btn.data("note-id");

        $(modalCommentBodyId).load("/Comment/ShowNoteComments/" + noteid);
    })

});

//Commentlemek için 
function doComment(btn, e, commentid, spanid) {

    var button = $(btn); //jquery butonuna çeviriyorum.
    var mode = button.data("edit-mode");

    if (e === "edit_clicked") {
        if (!mode) {  //Edit modum falsesa
            button.data("edit-mode", true);
            button.removeClass("btn-warning");
            button.addClass("btn-success");
            var btnSpan = button.find("span");
            btnSpan.removeClass("glyphicon-edit");
            btnSpan.addClass("glyphicon-ok");

            $(spanid).addClass("editable");
            $(spanid).attr("contenteditable", true);
            $(spanid).focus();
        }
        else { //bir daha tıklandığında
            button.data("edit-mode", false);
            button.addClass("btn-warning");
            button.removeClass("btn-success");
            var btnSpan = button.find("span");
            btnSpan.addClass("glyphicon-edit");
            btnSpan.removeClass("glyphicon-ok");

            $(spanid).removeClass("editable");
            $(spanid).attr("contenteditable", false);

            var txt = $(spanid).text();

            $.ajax({
                method: "POST",
                url: "/Comment/Edit/" + commentid,
                data: { text: txt }
            }).done(function (data) {

                if (data.result) {
                    // yorumlar partial tekrar yüklenir..
                    $(modalCommentBodyId).load("/Comment/ShowNoteComments/" + noteid);
                    //modalin comment body si içine bu url den note id si bu olan  commentleri çek getir.
                }
                else {
                    alert("Yorum güncellenemedi.");
                }

            }).fail(function () {
                alert("Sunucu ile bağlantı kurulamadı.");
            });
        }

    }
    else if (e === "delete_clicked") {
        var dialog_res = confirm("Yorum silinsin mi?");
        if (!dialog_res) return false; //eğer false dönerse akışı burada bitirip silme işlemini yapmıyorum.

        $.ajax({
            method: "GET",
            url: "/Comment/Delete/" + commentid
        }).done(function (data) {

            if (data.result) {
                // yorumlar partial tekrar yüklenir..
                $(modalCommentBodyId).load("/Comment/ShowNoteComments/" + noteid);
            } else {
                alert("Yorum silinemedi.");
            }

        }).fail(function () {
            alert("Sunucu ile bağlantı kurulamadı.");
        });

    } else if (e === "new_clicked") {

        var txt = $("#new_comment_text").val();

        $.ajax({
            method: "POST",
            url: "/Comment/Create", //Create methoduna post yapacağım
            data: { "text": txt, "noteid": noteid } //tırnak koydum çünkü property ve değişkeni anlaması için
        }).done(function (data) {

            if (data.result) {
                // yorumlar partial tekrar yüklenir..
                $(modalCommentBodyId).load("/Comment/ShowNoteComments/" + noteid);
            } else {
                alert("Yorum eklenemedi.");
            }

        }).fail(function () {
            alert("Sunucu ile bağlantı kurulamadı.");
        });

    }

}