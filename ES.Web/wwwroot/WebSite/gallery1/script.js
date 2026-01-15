// Fit inner div to gallery
$('<div />', { 'class': 'inner'  }).appendTo('.gallery');

// Create main image block and apply first img to it
var imageSrc1 = $('.gallery').children('img').eq(0).attr('src');
$('<div />', { 'class': 'main'  }).appendTo('.gallery .inner').css('background-image', 'url(' + imageSrc1 + ')');

// Create image number label
var noOfImages = $('.gallery').children('img').length;
$('<span />').appendTo('.gallery .inner .main').html('Image 1 of ' + noOfImages);

// Create thumb roll
$('<div />', { 'class': 'thumb-roll'  }).appendTo('.gallery .inner');

// Create thumbail block for each image inside thumb-roll
$('.gallery').children('img').each( function() {
	var imageSrc = $(this).attr('src');
	$('<div />', { 'class': 'thumb'  }).appendTo('.gallery .inner .thumb-roll').css('background-image', 'url(' + imageSrc + ')');
});

// Make first thumbnail selected by default
$('.thumb').eq(0).addClass('current');

// Select thumbnail function
$('.thumb').click(function() {
	
	// Make clicked thumbnail selected
	$('.thumb').removeClass('current');
	$(this).addClass('current');
	
	// Apply chosen image to main
	var imageSrc = $(this).css('background-image');
	$('.main').css('background-image', imageSrc);
	$('.main').addClass('main-selected');
	setTimeout(function() {
		$('.main').removeClass('main-selected');
	}, 500);
	
	// Change text to show current image number
	var imageIndex = $(this).index();
	$('.gallery .inner .main span').html('Image ' + (imageIndex + 1) + ' of ' + noOfImages);
});

// Arrow key control function
$(document).keyup(function(e) {

  // If right arrow
  if (e.keyCode === 39) {

	// Mark current thumbnail
	var currentThumb = $('.thumb.current');
	var currentThumbIndex = currentThumb.index();
	if ( (currentThumbIndex+1) >= noOfImages) { // if on last image
		nextThumbIndex = 0; // ...loop back to first image
	} else {
		nextThumbIndex = currentThumbIndex+1;
	}
	var nextThumb = $('.thumb').eq(nextThumbIndex);
	currentThumb.removeClass('current');
	nextThumb.addClass('current');
	
	// Switch main image
	var imageSrc = nextThumb.css('background-image');
	$('.main').css('background-image', imageSrc);
	$('.main').addClass('main-selected');
	setTimeout(function() {
		$('.main').removeClass('main-selected');
	}, 500);
	
	// Change text to show current image number
	$('.gallery .inner .main span').html('Image ' + (nextThumbIndex+1) + ' of ' + noOfImages);
	
  }
  
  // If left arrow
  if (e.keyCode === 37) { 

	// Mark current thumbnail
	var currentThumb = $('.thumb.current');
	var currentThumbIndex = currentThumb.index();
	if ( currentThumbIndex == 0) { // if on first image
		prevThumbIndex = noOfImages-1; // ...loop back to last image
	} else {
		prevThumbIndex = currentThumbIndex-1;
	}
	var prevThumb = $('.thumb').eq(prevThumbIndex);
	currentThumb.removeClass('current');
	prevThumb.addClass('current');
	
	// Switch main image
	var imageSrc = prevThumb.css('background-image');
	$('.main').css('background-image', imageSrc);
	$('.main').addClass('main-selected');
	setTimeout(function() {
		$('.main').removeClass('main-selected');
	}, 500);
	
	// Change text to show current image number
	$('.gallery .inner .main span').html('Image ' + (prevThumbIndex+1) + ' of ' + noOfImages);
	
  }
    
});