(function($) {
	
	"use strict";
	
	// Cache Selectors
	var mainWindow		=$(window),
		mainDocument	=$(document),
		myLoader		=$(".loader"),
		myNav			=$(".main-navbar"),
		mytopBar		=$('#top-bar'),
		searchBtn		=$(".search-button"),
		closeBtn		=$("#close-button"),
		closeButn		=$("#closebtn"),
		menuBtn			=$("#menu-button"),
		mySidenav		=$("#mySidenav"),
		overlay			=$(".overlay"),
		colorPanel		=$('#colorPanel');
	
	
	// Loader
	mainWindow.on('load', function () {
		myLoader.fadeOut("slow");
	});
	
	
	// Navbar
	mainDocument.on('ready', function(){
	
		myNav.affix({
			offset: { 
				top: function() { return mytopBar.height(); }
			}
		});
	});
	
	mainDocument.on('ready',function(){
		searchBtn.click(
		function(){
			overlay.css('transform','translateY(0%)');
		});
	});
	
	mainDocument.on('ready',function(){
		closeBtn.click(
		function(){
			overlay.css('transform','translateY(-120%)');
		});
	});
	
	mainDocument.on('ready',function(){
		menuBtn.on('click',
		function(){
			mySidenav.css('transform','translateX(0%)');
		});
	});
	
	mainDocument.on('ready',function(){
		closeButn.on('click',
		function(){
			mySidenav.css('transform','translateX(120%)');
		});
	});
	
	
	// Scroll
	$(document).ready(function(){
	
	  // Add smooth scrolling to all links
	  $(".landing-page-navbar a").on('click', function(event) {
	
		// Make sure this.hash has a value before overriding default behavior
		if (this.hash !== "") {
		  // Prevent default anchor click behavior
		  event.preventDefault();
	
		  // Store hash
		  var hash = this.hash;
	
		  // Using jQuery's animate() method to add smooth page scroll
		  // The optional number (800) specifies the number of milliseconds it takes to scroll to the specified area
		  $('html, body').animate({
			scrollTop: $(hash).offset().top
		  }, 800, function(){
	   
			// Add hash (#) to URL when done scrolling (default click behavior)
			window.location.hash = hash;
		  });
		} // End if
	  });
	});

	// Color Picker
	mainDocument.on('ready', function(){
		colorPanel.ColorPanel({
			styleSheet: '#cpswitch'
            , colors: {
                '#faa61a': 'css/orange.css'
                , '#00adef': 'css/lightblue.css'
				, '#a6ce39': 'css/green.css'
                , '#e62a2b': 'css/red.css'
                , '#cc6699': 'css/purple.css'
				, '#00cc99': 'css/caribbean-green.css'
				, '#00cccc': 'css/egg-blue.css'
				, '#ff884d': 'css/atomic.css'
            , }
            , linkClass: 'linka'
            , animateContainer: false
		});
	});

})(jQuery);
