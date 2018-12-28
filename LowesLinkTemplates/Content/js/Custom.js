$(document).ready(function () {
    //removing blank spaces from the content
    $('.page-content').html($('.page-content').html().replace(/\u200B/g, ''));
    $('.page-content').html($('.page-content').html().replace(/&nbsp;/g, ' '));
    //dropdown show styles & functionality
    $(".dropdown").on("show.bs.dropdown", function () {
        $(this)
            .find(".dropdown-menu")
            .first()
            .stop(true, true)
            .fadeIn();
    });
    //dropdown hide styles & functionality
    $(".dropdown").on("hide.bs.dropdown", function () {
        $(this)
            .find(".dropdown-menu")
            .first()
            .stop(true, true)
            .fadeOut();
    });
    //Partner Information Dropdown click functionality
    $("#partnerAnch").on("click", function (e) {
        e.preventDefault();
        $("#partnerInfoDropdown").toggleClass("show");
    });
    //Toggle Left Nav click functionality
    $("#toggle-btn").on("click", function (e) {
        e.preventDefault();
        $(this).toggleClass("active");

        $(".side-navbar").toggleClass("shrinked");
        $(".content-inner").toggleClass("active");
        $(document).trigger("sidebarChanged");

        if ($(window).outerWidth() > 1183) {
            if ($("#toggle-btn").hasClass("active")) {
                $(".navbar-header .brand-small").hide();
                $(".navbar-header .brand-big").show();
                $("#sideContentDiv").css("display", "block");
            } else {
                $(".navbar-header .brand-small").show();
                $(".navbar-header .brand-big").hide();
                $("#sideContentDiv").css("display", "none");
                $(".list-unstyled li a").css("color", "#fff");
                $(".list-unstyled li a i").css("color", "#fff");
            }
        }
        if ($(window).outerWidth() < 1183) {
            $(".navbar-header .brand-small").show();
            console.log("3");
        }
    });

    //Highlighting Left Nav Links functionality
    var curPageHref = window.location.href;
    var curPageName = "";
    var curPageAnchorName = "";

    if (curPageHref.toLowerCase() == "http://loweslinkdev.azurewebsites.net/".toLowerCase() || curPageHref.toLowerCase() == "https://loweslinkdev.azurewebsites.net/".toLowerCase() || curPageHref.toLowerCase() == "http://loweslinkqa.azurewebsites.net/".toLowerCase() || curPageHref.toLowerCase() == "https://loweslinkqa.azurewebsites.net/".toLowerCase()) {
        curPageAnchorName = "Lowes Home";
        $("section#contentPageProducts ul li").each(function (i) {
            var currentLiTxt = $(this).html();
            $(this).html($.trim(currentLiTxt));
        });
    }
    else {
        curPageName = curPageHref.indexOf('/') ? curPageHref.substring(curPageHref.lastIndexOf("/") + 1) : "Home";;
        console.log(curPageName);
        curPageAnchorName = curPageName.indexOf('.aspx') ? curPageName.split('.')[0] : curPageName;
        if (curPageAnchorName.toLowerCase() == "Home".toLowerCase()) {
            curPageAnchorName = "Lowes Home";
        }
        if (curPageAnchorName.toLowerCase() == "Product_Information".toLowerCase()) {
            curPageAnchorName = "Lowes Product Information";
        }
        if (curPageAnchorName.toLowerCase() == "NewPartnerInformation".toLowerCase() || curPageAnchorName.toLowerCase() == "ExistingPartnerInformation".toLowerCase() || curPageAnchorName.toLowerCase() == "lgsourcing".toLowerCase()) {
            curPageAnchorName = "Lowes Partner Information";
        }
        if (curPageAnchorName.toLowerCase() == "Edi".toLowerCase()) {
            curPageAnchorName = "Lowes EDI";
        }
        if (curPageAnchorName.toLowerCase() == "Canada".toLowerCase()) {
            curPageAnchorName = "Lowes Canada";
        }
        if (curPageAnchorName.toLowerCase() == "Mexico".toLowerCase()) {
            curPageAnchorName = "Lowes Mexico";
        }
    }
    console.log(curPageAnchorName);
    $('nav.side-navbar ul a[name="' + curPageAnchorName + '"]').css({
        'background': '#FA7146',
        'border-left': '4px solid #0072AF',
        'color': '#fff'
    });
    //Padding for inner-content & Removing trailing spaces from li's
    if (curPageAnchorName.toLowerCase() != "Lowes Home".toLowerCase())
    {
        if ($('div.ms-rte-layoutszone-inner').length == 0) {
            $('div#pageCommonContent > div').css({
                'padding-left': '20px',
                'padding-top': '10px',
                'margin-left': '20px',
                'margin-bottom': '20px',
                'color': 'black'
            });
            $('div#pageCommonContent > div h3').parent('div').css({
                'border': '2px solid'                
            });
            $('div#pageCommonContent > div > span,p').css({
                'font-family': 'Verdana, Helvetica',
                'font-size': 'small'
            });
            $('div#pageCommonContent > div > strong').css({
                'font-family': 'Verdana, Helvetica',
                'font-size': 'small'
            });
            $('div#pageCommonContent > div > h3 > p,span > strong').css({
                'font-family': 'Verdana, Helvetica',
                'font-size': 'small'
            });
            $("div#pageCommonContent > div > ul li").each(function (i) {
                var currentLiTxt = $(this).html();
                $(this).html($.trim(currentLiTxt)); 
            });
        }
        else {
            $('div.ms-rte-layoutszone-inner > div').css({
                'padding-left': '20px',
                'padding-top': '10px',
                'margin-left': '20px',
                'margin-bottom': '20px',
                'color': 'black'
            });
            $('div.ms-rte-layoutszone-inner > div h3').parent('div').css({
                'border': '2px solid'
            });                     
            $('div.ms-rte-layoutszone-inner > div > span,p').css({
                'font-family': 'Verdana, Helvetica',
                'font-size': 'small'
            });
            $('div.ms-rte-layoutszone-inner > div > strong').css({
                'font-family': 'Verdana, Helvetica',
                'font-size': 'small'
            });
            $('div.ms-rte-layoutszone-inner > div > h3 > p,span > strong').css({
                'font-family': 'Verdana, Helvetica',
                'font-size': 'small'
            });
            $("div.ms-rte-layoutszone-inner > div > ul li").each(function (i) {
                var currentLiTxt = $(this).html();
                $(this).html($.trim(currentLiTxt));
            });
        }        
    }
    if (curPageAnchorName.toLowerCase() == "Lowes Home".toLowerCase()) {
        $("section#contentPageProducts ul li").each(function (i) {
            var currentLiTxt = $(this).html();
            $(this).html($.trim(currentLiTxt));
        });
        $("div.content-inner").css("padding-bottom", "10px");
    }
    // Show More/Less Links functionality on Home Page
    $("a#showMore").click(function () {
        if ($(this).prev().find("li").hasClass('hide')) {
            $(this).prev().find("li").removeClass("hide");
            $("section#contentPageProducts").css("height", "300px");
            //$(this).closest("section#contentPageProducts").css("height", "290px");            
            //$(this).parent("table").closest("td > section#contentPageProducts").css("height", "290px");
            $(this).text('Show Less');
        } else {
            $(this).prev().find("li:nth-child(3)").nextAll().addClass("hide");           
            $("section#contentPageProducts").css("height", "250px");            
            $(this).text('Show More');
        }
    });
});