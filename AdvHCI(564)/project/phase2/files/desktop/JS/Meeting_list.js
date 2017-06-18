var skata = angular.module('meeting_app',[])

.controller('meeting_app_controller',function($scope){

	$scope.list=[
			{name:'Knights of the round table',date:'5/11/2015 16:00',place:'Meeting room',status:'Starting',img:'1443455160_linux-server-system-platform-os-computer-penguin.png',admin:'Admin: Stayros Dag',attend:'Attending'},
			{name:'Knights of the Square table',date:'5/10/2015 15:00',place:'Meeting room',status:'Pending',img:'Shield_logo.jpg',admin:'Admin: Stayros Dag',attend:'Attending'},
			{name:'Ti skotose o Ai Giorgis',date:'8/10/2015 16:00',place:'Meeting room',status:'Completed',img:'Corp_logo.png',admin:'Admin: Kwstas Gray',attend:'Remote Attended',desc:"o mparmpamprilios"}
			//{name:'Rolo aspida',date:'25/11/2015 16:00',place:'Meeting room',status:'Starting',img:'1443455160_linux-server-system-platform-os-computer-penguin.png',admin:'Admin: Stayros Dag',attend:'Attending'}
			];				
	$scope.predicate = 'name';
    $scope.reverse = false;
    $scope.order = function(predicate) {
        $scope.reverse = ($scope.predicate === predicate) ? !$scope.reverse : true;
        $scope.predicate = predicate;
	};
	$scope.setFile = function(element){
		$scope.$apply(function($scope) {
            $scope.theFile = element.files[0];
        });
	};
	$scope.submit = function(meeting){
		console.log('before: ' + $scope.list.length);
		if ($scope.theFile == null)
			meeting.img = "Corp_logo.png";
		else
			meeting.img = $scope.theFile.name;
			
		if (meeting.place == null){
			meeting.place = "Meeting room";
		}
		$scope.list.push({name: meeting.name, date: meeting.datetime, status: "Pending", admin: "Kwstas Mad", attend: "Attending", img: meeting.img, place: meeting.place, desc: meeting.desc });
		console.log('after:' + $scope.list.length);
		console.log($scope.list);

		$scope.showOverlay = false;
	};
});