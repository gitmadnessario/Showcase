$( document ).ready(function() {

		//Initialize kinect communication options.
		kinectHandler.InitializeKinectCommunication();
		
		//Set up button callbacks

	    $( "#Button1" ).click(function() {
			console.log($(this).attr("id")+" pressed");
		});
		
		$( "#Button2" ).click(function() {
			console.log($(this).attr("id")+" pressed");
		});

		
		$( "#Button3" ).click(function() {
			console.log($(this).attr("id")+" pressed");
		});

		
		$( "#Button4" ).click(function() {
			console.log($(this).attr("id")+" pressed");
		});

});

//Socket handler, responsible for all socket operations.
var socketHandler = {

	//Function waiting till socket connection is ready,
	//then triggering a callback.
	WaitForConnection: function (callback, interval) {
	    if (window.webSocket.readyState === 1) {
	        callback();
	    } else {
	        var that = this;
	        // optional: implement backoff for interval here
	        setTimeout(function () {
	            that.WaitForConnection(callback, interval);
	        }, interval);
	    }
	},

	//Function sending a message using web socket,
	//and calling an after-effect callback if available.
	Send: function (message, callback) {
	    this.WaitForConnection(function () {
	        window.webSocket.send(message);
	        if (typeof callback !== 'undefined') {
	          callback();
	        }
	    }, 1000);
	}


};

//Kinect handler, responsible for all kinect operations.
var kinectHandler = {

	socketURL: 'ws://localhost:8181/',

	//Function for initializing speech.
	InitializeSpeechRecognizer: function(socket){

		var messageToServer = {
			//Add your words for speech recognition in the array here.
			messageType: "Speech",
			contents: ['LINA Previus Slide', ' LINA Next Slide','LINA volume','LINA lights','LINA temperature',
			'LINA close settings','LINA Enter fullscreen','LINA exit fullscreen','LINA start meeting','LINA end meeting',
			'LINA display John Smith','LINA display presentation'
			]
		};

		socketHandler.Send(JSON.stringify(messageToServer));
	},

	InitializeKinectCommunication: function()
	{
		var wsImpl = window.WebSocket || window.MozWebSocket;

		//Create a new websocket and connect
		window.webSocket = new wsImpl(this.socketURL);
		var that = this;


		
		//When data is comming from the server, this metod is called.
		window.webSocket.onmessage = function (evt) {
		    //console.log(evt.data);
			var kinectInfo = JSON.parse(evt.data);
			

		    //Passing parameters reverted if data valid.
			if (kinectInfo.x > 0 && kinectInfo.y > 0) {
			    graphicsHandler.CursorCoordinates(kinectInfo.y, kinectInfo.x, kinectInfo.z);
			}



			that.GestureController(kinectInfo.gesture);
			that.SpeechController(kinectInfo.speechText);
			
			if (kinectInfo.sensorInfo.isValid == "True") // 
			{
			    that.SensorsController(kinectInfo.sensorInfo);
			}

			var bodyVar = $("body");
			
			//console.log(kinectInfo.x + " " + kinectInfo.y);
			
			
		};

		//When the connection is established, this method is called.
		window.webSocket.onopen = function () {
			console.log("connection is established");
		};

		//When the connection is closed, this method is called.
		window.webSocket.onclose = function () {
			console.log("connection closed");
		}

		this.InitializeSpeechRecognizer(window.webSocket);
	},
	
	//Speech controller function.
	//TODO: extend it with your functionality.
	SpeechController: function(speechText)
	{
		if(speechText != '')
		{
			console.log(speechText);
		}

		if(speechText == 'LINA NEXT SLIDE'){

			$('#swipe_right').click();
		}

		if(speechText == 'LINA PREVIUS SLIDE'){
			$('#swipe_left').click();
		}

		if(speechText == 'LINA LIGHTS'){
			$('#close1').click();
			$('#close2').click();
			$('#settings').click();
		}

		if(speechText == 'LINA VOLUME'){
			$('#close').click();
			$('#close2').click();
			$('#settings1').click();
		}
		
		if(speechText == 'LINA TEMPERATURE'){
			$('#close').click();
			$('#close1').click();
			$('#settings2').click();
		}
		
		if(speechText == 'LINA CLOSE SETTINGS'){
			$('#close').click();
			$('#close1').click();
			$('#close2').click();

		}

		if(speechText == 'LINA ENTER FULLSCREEN'){
			$('#full').click();
		}

		if(speechText == 'LINA EXIT FULLSCREEN'){
			$('#full').click();
		}

		if(speechText == 'LINA START MEETING'){
			$('#idle_but').click();

		}

		if(speechText == 'LINA END MEETING'){
			$('#end_meeting').click();
			
		}
		/*'LINA display Kostas Madness','LINA display presentation*/

		if(speechText == 'LINA DISPLAY JOHN SMITH'){
			
			$('#display_con').click();
		}

		if(speechText == 'LINA DISPLAY PRESENTATION'){
			
			$('#display_pre').click();
		}
	},

	//Gestures controller function.
	//TODO: extend it with your functionality.
	GestureController: function(gesture)
	{
		if(gesture != '')
		{
			console.log(gesture);
		}
		
		if(gesture =='SwipeUpLeft'){
			console.log("Epiasa swipe");
			$('#swipe_left').click();
		}
		if(gesture =='SwipeDownLeft'){
			console.log("Epiasa swipe");
			$('#swipe_left').click();
		}

		if(gesture =='SwipeLeft'){
			console.log("Epiasa swipe");
			$('#swipe_left').click();
			/*console.log(document.getElementById('swipe_left'));*/

		}

		if(gesture == 'WaveLeft'){
			console.log("Epiasa wave left");
			$('#swipe_left').click();
			
		}

		if(gesture =='SwipeUpRight'){
			console.log("Epiasa swipe");
			$('#swipe_right').click();
		}
		
		if(gesture =='SwipeDownRight'){
			$('#swipe_right').click();
		}
		
		if(gesture =='SwipeRight'){
			$('#swipe_right').click();
		}
		

		if(gesture == 'WaveRight'){
			console.log("Epiasa wave right");
			$('#swipe_right').click();
		}

		if(gesture == 'ZoomIn'){
			$('#full').click();
		}
		
		if(gesture == 'ZoomOut'){
			$('#full').click();
		}
	},
	
	SensorsController: function(sensor)
	{
	    console.log("Sensor index: " + sensor.index + " value: " + sensor.value);

	    //Diko mas kwdhkas
	    var temp =0; 
	    var flag =0;
	    if(sensor.index == 1 ||sensor.index ==0){
	    		    var dimer= sensor.value/10;

	    		    if(!flag){
    	    		    console.log("Typwnw lew :"+dimer);
    	    		    $('.progress_bar').attr("style","width:"+dimer+"%");
    	    		    document.getElementsByClassName('test').innerHTML=dimer+'%';
    	    		    if(dimer == 0){
	    		    		flag = 1;
	    		    	}
   					}else{

   						if(dimer<temp){
   							flag = 1;
   							temp = dimer;
   						}else {flag = 0;}

   					}

	    		}
        //If close touch sensor is touched....
	    if(sensor.index == 2 && sensor.value == 0)
	    {

	        console.log("Sensor Touched!");
	        $('#display_pre').click();
            /*//Get int from 0 to 7.
	        var randomNumber = Math.floor(Math.random() * 7);
	        console.log(randomNumber);
            //convert to binary string.
	        var binaryString = (randomNumber >>> 0).toString(2);
	        var messageToServer = {
	            //Add your words for speech recognition in the array here.
	            messageType: 'Light',

	            //Getting byte and passing as [R, G, B] bytes, comparing it to 1 for true.
	            contents: [ Number(binaryString[2] !== undefined ? binaryString[2] : 0) == 1,
                            Number(binaryString[1] !== undefined ? binaryString[1] : 0) == 1,
                            Number(binaryString[0]) == 1 ]
	        };*/
	        
            //Send message to server.
	        socketHandler.Send(JSON.stringify(messageToServer));
	    }
	}

};

//Graphics handler, responsible for all socket operations.
var graphicsHandler = {

	menuUILock: false,
	loadingInterval: null,

	EnableMainButton: function(button)
	{
		if(!this.menuUILock){
			this.menuUILock = true;
			$(".progressLoading").empty();
			var CircularLoader  = new ProgressBar.Circle('.progressLoading', {
								color: '#FFFF7A',
								strokeWidth: 10,
								easing: 'easeInOut'
			});
			CircularLoader.animate(1,{duration: 3000}, function() {
				//console.log('Animation has finished');
			});
			
			this.loadingInterval = setTimeout(function() {
				this.menuUILock = false;
				$(".progressLoading").empty();
				button.click();
			}, 3000);
		}	
	},


	//Validate that cursor should be visible.
	ValidateCursorAppearance: function(selector)
	{
		var numItems = $(selector).length;
		if(numItems != 1)
		{
				clearTimeout(this.loadingInterval);
				this.menuUILock = false;
				$(".progressLoading").empty();
		}
	},



	//Renders cursor on browser.
	CursorCoordinates: function(x,y,z)
	{
		$("#cursor").css({ "top": x, "left": y}).fadeIn('fast');
		//console.log(x + " " + y);
		this.CursorIsOnHover(z);
	},


	//Collision detection function.	
	Collision: function($div1, $div2) {
		try {

			var objTop 	   = $div1.offset().top,
				objLeft    = $div1.offset().left,
				objWidth   = $div1.width(),
				objHeight  = $div1.height();  
				
			var	selfLeft   = $div2.offset().left,
				selfTop    = $div2.offset().top,
				selfWidth  = $div2.width(),
				selfHeight = $div2.height();

			return ((objLeft + objWidth) > selfLeft   &&
					 objLeft < (selfLeft + selfWidth) &&
			   	    (objTop  + objHeight) > selfTop   &&
					 objTop  < (selfTop + selfHeight));

		}
		catch(err) {
			console.log( err.message);
			return false;
		}
	},			

	//Checks if cursor is on hover of some elements.
	CursorIsOnHover: function(z)
	{
		var my 	 = $('#cursor');
		var that = this;   

		//Here, the check is done on a number of buttons,
		//having the "buttons" class.
		$('.buttons').each(function(e){
				var self = $(this);
				
				if(that.Collision(self, my))
				{
					self.css('color','red');
					self.addClass("hover");
					that.EnableMainButton(self);
				}		
				else 
				{
					self.css('color','black');
					self.removeClass("hover");
				}
				that.ValidateCursorAppearance(".hover");
		});

		$('.carousel-control').each(function(e){
				var self = $(this);
				
				if(that.Collision(self, my))
				{
					self.css('color','red');
					self.addClass("hover");
					that.EnableMainButton(self);
				}		
				else 
				{
					self.css('color','black');
					self.removeClass("hover");
				}
				that.ValidateCursorAppearance(".hover");
		});
	}

};

