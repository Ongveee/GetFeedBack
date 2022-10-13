var Main = {
    UpEvent: function () {
        $('.select2').each(function () {
            $(this).select2({
                width: '100%'
            });
        });
        Utils.updateInputDate(jQuery(document));
    },
    onEvent: function () {
        jQuery(document).on("change", "select.cust_select_change", function () {
            var select = jQuery(this);
            if (select.val() > 0) {
                var target2 = select.data('target2');
                var target = jQuery(select.data('target'));
                var _target = target.data('target');
                jQuery.ajax({
                    type: "POST",
                    async: true,
                    url: select.data('url'),
                    data: { id: select.val() },
                    success: function (response) {
                        target.html(response.data);
                        target.find(".select2").select2({
                            width: '100%'
                        });
                        if (_target != undefined && _target != '') {
                            target.find('select').attr('data-target', _target);
                        }

                        if (target2 != undefined && target2 != '') {
                            jQuery(target2).html(response.data2);
                            $(target2).find(".select2").select2({
                                width: '100%'
                            });
                        }

                        //Main.onEvent();
                    }
                });
            }
            return false;
        });

        jQuery(document).on("change", "#handlingProcedure", function () {
            var select = jQuery(this);
            let target = $("#table_TP_HS_TTHC");
            if (select.val() > 0) {
                $('#spproduce_title').text($('#handlingProcedure option:selected').text());
                $('#produce_title').val($('#handlingProcedure option:selected').text());
                jQuery.ajax({
                    type: "POST",
                    async: true,
                    url: "/AutoComplete/GetThuTucHanhChinh?id=",
                    data: { id: select.val() },
                    success: function (rs) {
                        target.html(rs.data);
                    }
                });
            }
            return false;
        });

        jQuery(document).on("change", "#CoQuan", function () {
            var select = jQuery(this);
            if (select.val() > 0) {
                $('#spOrganName').text($('#CoQuan option:selected').text());
                $('#OrganName').val($('#CoQuan option:selected').text());

                //lấy mã định danh cơ quan
                var code = $('#CoQuan option:selected').data('code');
                //lấy ngày hiện tại 
                var today = new Date();
                var currentDate = today.toISOString().substring(0, 10);
                var arrayDate = currentDate.split("-");
                var profileDate = arrayDate[0].substring(2, 4) + arrayDate[1] + arrayDate[2]
                //gán tạm thời random
                var xx = Math.floor(Math.random() * 10001);

                $('#spMaHoSo').text(code + " - " + profileDate + " - " + xx);
                $('#MaHoSo').val(code + " - " + profileDate + " - " + xx);

                $('#spMaBienNhan').text(xx + " / " + arrayDate[0]);
                $('#MaBienNhan').val(xx + " / " + arrayDate[0]);

                $('#spMaTraCuu').text(profileDate + xx);
                $('#MaTraCuu').val(profileDate + xx);

            }
            return false;
        });
    },
    EventDuplicateAddress: function () {
        var tinh = $('#Tinh').val();
        var huyen = $('#Huyen').val();
        var xa = $('#Xa').val();
        var diachi = $('#Address').val();
        if (tinh != '0') {
            $('#Tinh2').select2("val", tinh);

            $('#Huyen2').val(huyen);
            $('#Xa2').val(xa);
            $('#Address2').val(diachi);
        }
    },
    SetMessage: function (res) {
        if (res.code != undefined)
            res.code = res.code.toLowerCase();
        if (res.code === "success") {
            CommonJs.ShowNotifyMsg(res.code, res.message);
            setTimeout(function () {
                window.location.reload();
            }, 1000);
        }
        else {
            Main.ShowNotifyMsg(res.code, res.message);
        }
    },
    ShowNotifyMsg: function (msgType, msgContent) {
        if (msgType != undefined)
            msgType = msgType.toLowerCase();
        let bgClass = "";
        if (msgType === "success")
            bgClass = 'bg-success';
        else if (msgType === "error")
            bgClass = 'bg-danger';
        else if (msgType === "warning")
            bgClass = 'bg-warning';
        else if (msgType === "info")
            bgClass = 'bg-info';

        $(document).Toasts('create', {
            class: bgClass,
            autohide: true,//res.type === "Success",
            delay: 5000,
            position: 'bottomRight',
            title: "Thông báo",
            body: msgContent
        });
        return false;
    }
}

//--DOCUMENT READY FUNCTION BEGIN
$(document).ready(function () {
    // $("").click(function(){
    // });
    // =========== BACK TO TOP ==========
    $("#back-to-top").click(function () {
        $("body,html").animate({
            scrollTop: 0
        }, 800);
        return false;
    });

    jQuery("select.selected_linkweb").on("change", function () {
        jQuery("select.selected_linkweb option:selected").each(function () {
            var target = jQuery(this).data("target");
            var url = jQuery(this).data("url");
            if (url != undefined && url.length > 0) {
                window.open(url, target);
            }
        });
    });


    jQuery(".btn-menu-canvas").click(function () {
        if (jQuery("#offcanvas").hasClass("active")) {
            jQuery("body").removeClass("off-canvas-active");
            jQuery("#offcanvas").removeClass("active");
            jQuery(".wrapper-container").removeClass("offcanvas-push");
        } else {
            jQuery("body").addClass("off-canvas-active");
            jQuery("#offcanvas").addClass("active");
            jQuery(".wrapper-container").addClass("offcanvas-push");
        }
    });
    jQuery("#off-canvas-button").click(function () {
        jQuery("#offcanvas").removeClass("active");
        jQuery(".wrapper-container").removeClass("offcanvas-push");
    });
    jQuery(document).mouseup(function (e) {
        var container = jQuery("#offcanvas");
        if (!container.is(e.target) // if the target of the click isn't the container...
            && container.has(e.target).length === 0) // ... nor a descendant of the container
        {
            jQuery("#offcanvas").removeClass("active");
            jQuery(".wrapper-container").removeClass("offcanvas-push");
        }
    });
    jQuery("#offcanvas .navbar-nav ul").hide();
    jQuery("#offcanvas .navbar-nav li h3 i").addClass("accordion-show");
    jQuery("#offcanvas .navbar-nav li h3 i").click(function () {
        if (jQuery(this).parent().next().is(":visible")) {
            jQuery(this).addClass("accordion-show");
        } else {
            jQuery(this).removeClass("accordion-show");
        }
        jQuery(this).parent().next().toggle(400);
        if (jQuery(this).hasClass("arrow_carrot-right")) {
            jQuery(this).removeClass("arrow_carrot-right");
            jQuery(this).addClass("arrow_carrot-down");
        } else {
            jQuery(this).removeClass("arrow_carrot-down");
            jQuery(this).addClass("arrow_carrot-right");
        }
    });



});
//--DOCUMENT READY FUNCTION END
jQuery(window).scroll(function () {
    // // =========== STICKY MENU ==========
    // var posScroll = jQuery(window).scrollTop();
    // //Lấy vị trí hiện tại của menu cách top x px
    // //pos = jQuery(".menu").position();   
    // // var scroll = jQuery(window).scrollTop();
    // //if (parseInt(posScroll) > parseInt(pos.top)) {
    // if ($(window).width() > 750) {
    //     if (parseInt(posScroll) > 98) {
    //         jQuery("#site-header .header").addClass("is-ticky");
    //     }else{
    //         jQuery("#site-header .header").removeClass("is-ticky");
    //     }
    // }else{
    //     if (parseInt(posScroll) > 0) {
    //         jQuery("#site-header .header").addClass("is-ticky");
    //     }else{
    //         jQuery("#site-header .header").removeClass("is-ticky");
    //     }
    // }

    // =========== BACK TO TOP ==========
    if ($(this).scrollTop() > 50) {
        $("#back-to-top").fadeIn(600);
    } else {
        $("#back-to-top").fadeOut(600);
    }

});




function display(view) {
    if (view == "grid") {
        $(".product-grid-show").removeClass("list_over");
        $(".display").html('<button class="active " id="grid" rel="tooltip" title="Dạng Lưới" onclick="display(\'grid\');"><i class="fa fa-th-large"></i></button> <button class="" id="list" rel="tooltip" title="Dạng Danh sách" onclick="display(\'list\');"><i class="fa fa-th-list"></i></button>');
        localStorage.setItem("display", "grid");
    } else {
        $(".product-grid-show").addClass("list_over");
        $(".display").html('<button class="" id="grid" rel="tooltip" title="Dạng Lưới" onclick="display(\'grid\');"><i class="fa fa-th-large"></i></button> <button class="active " id="list" rel="tooltip" title="Dạng Danh sách" onclick="display(\'list\');"><i class="fa fa-th-list"></i></button>');
        localStorage.setItem("display", "list");
    }
}
if (localStorage.getItem("display") == "list") {
    display("list");
} else {
    display("grid");
};
$(".modal-review__rating-order-wrap > span").click(function () {
    $(this).addClass("active").siblings().removeClass("active");
    $(this).parent().attr("data-rating-value", $(this).data("star-index"));
    $(this).parent().next(".comment-content__vote-total").text(`${$(this).data("star-index")}/5`);
});
jQuery(document).on("submit", ".quickSubmitLogin", function (e) {
    e.preventDefault();
    var form = jQuery(this);
    try {
        if (!form.hasClass("submitting")) {
            form.addClass("submitting");
            var url = form.attr("action");
            var target = form.attr("data-target");
            var targetDelete = form.attr("data-target-delete");
            var type = form.attr("data-insert-type");
            var data = Utils.getSerialize(form);
            if (Utils.isEmpty(url)) {
                form.removeClass("submitting");
                return false;
            }
            if (!Utils.validateDataForm(form)) {
                form.removeClass("submitting");
                return false;
            }

            jQuery.ajax({
                type: "POST",
                async: true,
                url: url,
                data: data,
                beforeSend: function () {
                },
                complete: function () {
                    form.removeClass("submitting");
                },
                error: function () {
                    form.removeClass("submitting");
                },
                success: function (response) {
                    form.removeClass("submitting");
                    if (!response.success) {
                        notifyError(response.message);
                    }
                    else {
                        notifySuccess(response.message);
                        setTimeout(function () {
                            window.location.reload();
                        }, 3000);
                    }
                }
            });
        }

    } catch (e) {
        console.log(e);
        form.removeClass("submitting");
    }
    return false;
});
jQuery(document).on("click", ".quickDownload", function (e) {
    e.preventDefault();
    var form = jQuery(this);
    try {

        Utils.lazyLoadAjax($(form.attr("data-target")));
        if (!form.hasClass("submitting")) {
            form.addClass("submitting");
            jQuery.ajax({
                type: "POST",
                async: true,
                url: form.attr("href"),
                beforeSend: function () {
                },
                complete: function () {
                    form.removeClass("submitting");
                },
                error: function () {
                    form.removeClass("submitting");
                },
                success: function (response) {
                    Utils.lazyLoadAjaxHide($(form.attr("data-target")));
                    form.removeClass("submitting");
                    if (response.success) {
                        Utils.SaveFileAs(response.urlFile, response.filename);
                        return false;
                    }
                    else {
                        notifyError(response.message);
                        setTimeout(function () {
                            window.location.reload();
                        }, 2000);
                    }
                }
            });
        }

    } catch (e) {
        form.removeClass("submitting");
        console.log(e);
    }
    return false;
});
jQuery(document).on("submit", ".quickSearch", function () {
    try {
        debugger
        var form = jQuery(this);
        var url = form.attr("action");
        var target = form.attr("data-target");
        var data = Utils.getSerialize(form);
        if (Utils.isEmpty(url)) {
            return;
        }
        if (!form.hasClass("submitting")) {
            form.addClass("submitting");
            jQuery.ajax({
                type: "POST",
                async: true,
                url: url,
                data: data,
                beforeSend: function () {
                    Utils.lazyLoadAjax();
                },
                complete: function () {
                    form.removeClass("submitting");
                    Utils.lazyLoadAjaxHide();
                },
                error: function () {
                    form.removeClass("submitting");
                    Utils.lazyLoadAjaxHide();
                },
                success: function (response) {
                    form.removeClass("submitting");
                    try {
                        window.history.pushState(null, response.title, url + Utils.builderQString(data));
                        jQuery(document).prop("title", response.title);
                    } catch (e) {
                        console.log(e);
                    }
                    jQuery(target).html(response.htCust);
                }

            });
        }
    } catch (e) {
        console.log(e);
        form.removeClass("submitting");
    }
    return false;
});
jQuery(document).on("submit", ".quickSearchModal", function () {
    try {
        debugger
        var form = jQuery(this);
        var url = form.attr("action");
        var target = form.attr("data-target");
        var data = Utils.getSerialize(form);
        if (Utils.isEmpty(url)) {
            return;
        }
        if (!form.hasClass("submitting")) {
            form.addClass("submitting");
            jQuery.ajax({
                type: "POST",
                async: true,
                url: url,
                data: data,
                beforeSend: function () {
                    Utils.lazyLoadAjax();
                },
                complete: function () {
                    form.removeClass("submitting");
                    Utils.lazyLoadAjaxHide();
                },
                error: function () {
                    form.removeClass("submitting");
                    Utils.lazyLoadAjaxHide();
                },
                success: function (response) {
                    form.removeClass("submitting");
                    jQuery(target).html(response.htCust);
                }

            });
        }
    } catch (e) {
        console.log(e);
        form.removeClass("submitting");
    }
    return false;
});
jQuery(document).on("click", ".quickAppend", function () {
    var form = jQuery(this);
    $(".quickAppend").removeClass('active');
    form.addClass("active");
    var url = form.attr("data-url");
    var target = form.attr("data-target");
    var data = form.attr("data-id");
    jQuery.ajax({
        type: "POST",
        async: true,
        url: url,
        data: { id: data },
        beforeSend: function () {
            jQuery(target).html('');
            Utils.lazyLoadAjax();
        },
        complete: function () {
            Utils.lazyLoadAjaxHide();
        },
        error: function () {
            Utils.lazyLoadAjaxHide();
        },
        success: function (response) {
            try {

            } catch (e) {
                console.log(e);
            }
            jQuery(target).html(response);
        }

    });

    return false;
});

jQuery(document).on("click", ".quickAccess", function () {
    var form = jQuery(this);
    var url = form.attr("data-href");

    jQuery.ajax({
        type: "POST",
        async: true,
        url: url,
        data: {},
        beforeSend: function () {
            Utils.lazyLoadAjax();
        },
        complete: function () {
            Utils.lazyLoadAjaxHide();
        },
        error: function () {
            Utils.lazyLoadAjaxHide();
        },
        success: function (response) {
            if (response.success) {
                notifySuccess(response.message);
                setTimeout(function () {
                    window.location.reload();
                }, 3000);
            }
            else {
                notifyError(response.message);
            }
        }

    });

    return false;
});
jQuery(document).on("submit", ".quickSubmit", function (e) {
    e.preventDefault();
    try {
        var form = jQuery(this);
        if (!form.hasClass("submitting")) {
            form.addClass("submitting");
            var url = form.attr("action");
            var target = form.attr("data-target");
            var data = Utils.getSerialize(form);
            if (Utils.isEmpty(url)) {
                return false;
            }
            jQuery.ajax({
                type: "POST",
                async: true,
                url: url,
                data: data,
                beforeSend: function () {
                    Utils.lazyLoadAjax();
                },
                complete: function () {
                    Utils.lazyLoadAjaxHide();
                    form.removeClass("submitting");
                },
                error: function () {
                    Utils.lazyLoadAjaxHide();
                    form.removeClass("submitting");
                },
                success: function (response) {
                    Utils.lazyLoadAjaxHide();
                    if (response.isCust == 1) {
                        if ($('#tbody-attached tr td[colspan="7"]').length > 0) {
                            //todo fix tạm giá trị
                            $('#tbody-attached').empty();
                        }

                        jQuery(target).append(response.htCust);
                        let tbody = $('#tbody-attached > tr');
                        let all_row = tbody.length;
                        for (let i = 0; i < all_row; i++) {
                            let row = $(`#tbody-attached > tr:eq(${i})`);
                            row.find('td:eq(0)').text(i + 1);
                        }

                        $('#addAttached').modal('hide');
                        return false;
                    }
                    form.removeClass("submitting");
                    if (response.success) {
                        notifySuccess(response.message);
                        if (Utils.notEmpty(response.htmlUrl)) {
                            window.location.href = response.htmlUrl;
                            return false;
                        }

                        setTimeout(function () {
                            window.location.reload();
                        }, 3000);
                    }
                    else {
                        notifyError(response.message);
                    }
                }
            });
        }

    } catch (e) {
        form.removeClass("submitting");
        console.log(e);
    }
    return false;
});
jQuery(document).on("click", ".quickUpdate", function () {
    try {
        var obj = jQuery(this);
        var target = jQuery(this).attr("data-target");
        var url = jQuery(this).attr("href");

        cusmodal.ShowView(url, function () {
            Main.onEvent();
        });

    } catch (e) {

    }
    return false;
});
jQuery(document).on("click", ".submitFile", function () {
    try {
        var obj = jQuery(this);
        var target = jQuery(this).attr("data-target");
        var url = jQuery(this).attr("href");
        var form = jQuery(this).attr("data-form");
        var data = Utils.getSerialize($(form));
        if (Utils.isEmpty(url)) {

            return false;
        }

        jQuery.ajax({
            type: "POST",
            async: true,
            url: url,
            data: data,
            beforeSend: function () {
            },
            complete: function () {

            },
            error: function () {

            },
            success: function (response) {

                var htmlview = "";
                htmlview = "<button  type='button' data-path=" + response.FilePath + "  data-name=" + response.FileName + "  class='btn btn-defaut p-0 btnViewFile'>" +
                    "<i class='font-icon icon-Action'></i>" +
                    "</button> "

                if (!response.success) {
                    notifyError(response.message);
                }
                else {
                    $(target).append("<div class='member'><div class='display-inline_block'>" +
                        //"<i class='font-icon ghim icon-blue'></i>" +
                        "<span title='" + response.FileName + "'>" + response.FileName + "</span></div>" +
                        "<input readonly name='FileName" + response.type + "' class='fileNames' type='hidden' value='" + response.FileName + "' />" +
                        "<input name='FilePath" + response.type + "' class='filePaths' type='hidden' value='" + response.FilePath + "'/>" +
                        "<div class='display-inline_block float-right'>" + htmlview +
                        "<button type='button' class='btn btn-defaut p-0 delMember'>" +
                        "<i class='font-icon delete icon-gray'></i>" +
                        "</button> </div></div>"
                    );
                    notifySuccess(response.message);


                }
            }
        });
    } catch (e) {

    }
    return false;
});
jQuery(document).on("click", ".getInfo_CaNhan", function () {
    //Utils.lazyLoadAjax();
    var obj = jQuery(this);
    var url = '/home/GetViewInfo';
    var form = obj.attr("data-form");
    var target = obj.attr("data-target");
    var data = Utils.getSerialize($(form));
    if (Utils.isEmpty(url)) {
        return false;
    }
    jQuery.ajax({
        type: "POST",
        async: true,
        url: url,
        data: data,
        beforeSend: function () {
            Utils.lazyLoadAjax();
        },
        complete: function () {
            Utils.lazyLoadAjaxHide();
        },
        error: function () {
            Utils.lazyLoadAjaxHide();
        },
        success: function (response) {
            Utils.lazyLoadAjaxHide();
            $(target).html(response.html)
            $(target).find(".select2").select2({
                width: '100%'
            });
        }
    });

    //$('#FullName').val($('#FullName1').val());
    //$('#Code').val($('#Code1').val());
    //$('#NoiCap').val($('#NoiCap1').val());
    //$("#Tinh").select2().val($('#Tinh1').val()).trigger("change");
    //$("#Huyen").select2().val($('#Huyen1').val()).trigger("change");
    //$("#Xa").select2().val($('#Xa1').val()).trigger("change");
    //$('#Email').val($('#Email1').val());
    //$('#Address').val($('#Address1').val());
    Utils.lazyLoadAjaxHide();
});
jQuery(document).on("click", ".showHideRowChild", function () {
    var btn = jQuery(this);
    var table = btn.closest('table');
    var tr = btn.closest('tr');
    var stt = tr.data('stt');
    var status = tr.attr('data-status');
    if (status == "1") {
        table.find('tr.rowChild_' + stt).addClass('hidden');
        tr.attr('data-status', '0');
        btn.find('i').removeClass('icon-chevron-up').addClass('icon-chevron-down');
    }
    else {
        table.find('tr.rowChild_' + stt).removeClass('hidden');
        tr.attr('data-status', '1');
        btn.find('i').removeClass('icon-chevron-down').addClass('icon-chevron-up');
    }
});
jQuery(document).on("click", ".btnViewFile", function () {
    try {
        var form = jQuery(this);
        data = form.getData();
        Utils.lazyLoadAjax();
        cusmodal.ShowViewData('/Viewer/ViewFile', data, function () {
            Utils.ViewFilePdf();
            Utils.lazyLoadAjaxHide();
        })

    } catch (e) {
        console.log(e);
    }
    return false;
});

jQuery(document).ready(function () {
    Main.onEvent();
    Main.UpEvent();
});

function copyToClipboard(element) {
    var $temp = $("<input>");
    $("body").append($temp);
    $temp.val(element).select();
    document.execCommand("copy");
    $temp.remove();
    notifySuccess('Copy link thành công');
}

$(document).ready(function(){
  
    /* 1. Visualizing things on Hover - See next part for action on click */
    $('#stars li').on('mouseover', function(){
      var onStar = parseInt($(this).data('value'), 10); // The star currently mouse on
     
      // Now highlight all the stars that's not after the current hovered star
      $(this).parent().children('li.star').each(function(e){
        if (e < onStar) {
          $(this).addClass('hover');
        }
        else {
          $(this).removeClass('hover');
        }
      });
      
    }).on('mouseout', function(){
      $(this).parent().children('li.star').each(function(e){
        $(this).removeClass('hover');
      });
    });
    
    
    /* 2. Action to perform on click */
    $('#stars li').on('click', function(){
      var onStar = parseInt($(this).data('value'), 10); // The star currently selected
      var stars = $(this).parent().children('li.star');
      
      for (i = 0; i < stars.length; i++) {
        $(stars[i]).removeClass('selected');
      }
      
      for (i = 0; i < onStar; i++) {
        $(stars[i]).addClass('selected');
      }
      
      // JUST RESPONSE (Not needed)
      var ratingValue = parseInt($('#stars li.selected').last().data('value'), 10);
      var msg = "";
      if (ratingValue > 1) {
          msg = "Thanks! You rated this " + ratingValue + " stars.";
      }
      else {
          msg = "We will improve ourselves. You rated this " + ratingValue + " stars.";
      }
      responseMessage(msg);
    });
    
    
  });
