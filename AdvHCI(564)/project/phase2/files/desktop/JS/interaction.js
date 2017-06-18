angular.module('interactionApp',[])

.controller('interactionAppCtrl',['$scope', 'InteractionFactory',function($scope,socket){
	
	socket.on('testing:test', function (message) {
      console.log("eisai malakas");
    });

}])