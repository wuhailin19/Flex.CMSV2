$('.middle-link a').off('click').on('click',function (event) {
    //event.preventDefault();
    $(this).addClass('active').siblings().removeClass('active');
    //var hash = $(this).attr("href").split("#")[1];
    //animateScroll("#"+hash);
})
function animateScroll($elment) {
    var itop = $($elment).offset().top;
    $(".content-right-box").stop().animate({
        scrollTop: itop
    }, 300);
}