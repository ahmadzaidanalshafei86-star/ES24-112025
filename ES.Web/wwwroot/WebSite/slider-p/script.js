// SLICK
 $('#product-slider__main').slick({
  asNavFor: '#product-slider__nav',
  rows: 0,
  slidesToShow: 1,
  slidesToScroll: 1,
  arrows: true,
  prevArrow: '<span class="slick-prev"><</span>',
  nextArrow: '<span class="slick-next">></span>',
  fade: true,
  adaptiveHeight: true,
  autoplay: true,
  autoplaySpeed: 4000
});

$('#product-slider__nav').slick({
  asNavFor: '#product-slider__main',
  rows: 1,
  slidesToShow: 6,
  slidesToScroll: 1,
  focusOnSelect: true,
  adaptiveHeight: true,
  dots: false,
  arrows: false
});

$(".social-share").click(function() {
  $(this).toggleClass("is-visible");
});


// ZOOM
$('.zoom').zoom();

// STYLE GRAB
$('.zoom--grab').zoom({ on:'grab' });

// STYLE CLICK
$('.zoom--click').zoom({ on:'click' });	

// STYLE TOGGLE
$('.zoom--toggle').zoom({ on:'toggle' });

$('#product-slider__main').slickLightbox({
  itemSelector        : '.zoom img',
  navigateByKeyboard  : true,
  src: 'src',
  lazy: true
});