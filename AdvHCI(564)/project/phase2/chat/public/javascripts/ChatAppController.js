'use strict';

/* Controllers */
app.controller('ChatAppCtrl', ['$scope', 'ChatAppFactory', function ($scope, socket) {

    //var socket = io();
    // SocketFactory listeners
    // ================

    socket.on('init', function (data) {
      $scope.name = data.name;
      $scope.users = data.users;
    });

    socket.on('send:message', function (message) {
      $scope.messages.push(message);
    });

    socket.on('user:join', function (data) {
      //$scope.messages.push({
      // user: 'chatroom',
      //  text: 'User ' + data.name + ' has joined.'
      //});
      $scope.users.push(data.name);
    });

    // add a message to the conversation when a user disconnects or leaves the room
    socket.on('user:left', function (data) {
      //$scope.messages.push({
      //  user: 'chatroom',
      //  text: 'User ' + data.name + ' has left.'
      //});
      var i, user;
      for (i = 0; i < $scope.users.length; i++) {
        user = $scope.users[i];
        if (user === data.name) {
          $scope.users.splice(i, 1);
          break;
        }
      }
    });

    $scope.messages = [];
	$scope.testing = [];

    $scope.sendMessage = function () {
        socket.emit('send:message', {
          message: $scope.message
        });

        // add the message to our model locally
        $scope.messages.push({
          user: $scope.name,
          text: $scope.message
        });

        // clear message box
        $scope.message = '';
    };

}])

app.controller('interactionAppCtrl',['$scope', 'ChatAppFactory',function($scope,socket){
	
	 socket.on('testing:test', function (message) {
      console.log("arxidia");
	  console.log(message)
	  if (message === "a"){
		  document.getElementById('swipe_left').click();
		  $('#swipe_left').click();
	  }
	  else{
		  document.getElementById('swipe_right').click();
		  $('#swipe_right').click();
	  }
    }); 
	
	$scope.testing = function(){
		console.log("AEK");
		socket.emit('testing:test',{
			message: "eisai malakas"
		});
	};

}])


app.controller('meeting_participants',function($scope){

	$scope.list=[
			{name:'Stayros Dag (admin)',img:'images/Batman-wallpaper-1080x1920.jpg'},
			{name:'Kwstas Madness',img:'1443455160_linux-server-system-platform-os-computer-penguin.png'},
			{name:'Giannhs Kalaitzos',img:'images/d5f89d10412f3ca9814f9d5314c4b3f1c00e78661d9a94028223ae8492c503f3.jpg'},
			{name:'Conference Room',img:'images/Conference-Room-Setup-Styles-Design-Idea-9.jpg'}
			];	

    
});