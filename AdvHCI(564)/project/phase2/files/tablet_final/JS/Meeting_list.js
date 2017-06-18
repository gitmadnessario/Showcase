angular.module('meeting_app',[])

.controller('meeting_app_controller',function($scope){

	$scope.list=[
			{name:'Knights of the round table',date:'5/11/2015 16:00 Meeting room',status:'Pending',img:'1443455160_linux-server-system-platform-os-computer-penguin.png',admin:'Admin: Stayros Dag',attend:'Attending'},
			{name:'Knights of the Square table',date:'5/10/2015 15:00 Meeting room',status:'Completed',img:'Shield_logo.jpg',admin:'Admin: Stayros Dag',attend:'Attended'},
			{name:'To swsto anoiksa',date:'8/10/2015 16:00 Meeting room',status:'Completed',img:'Corp_logo.png',admin:'Admin: Kwstas Gray',attend:'Remote Attended'}
			];	

    $scope.partlist=[
            {Name:'Knights of the round table',status:'Pending',img:'plus_sign.png',attend:'Attending'},
            {Name:'Knights of the round table',status:'Pending',img:'1443455160_linux-server-system-platform-os-computer-penguin.png',attend:'Attending'},
            {Name:'Knights of the round table',status:'Pending',img:'1443455160_linux-server-system-platform-os-computer-penguin.png',attend:'Attending'},
            ];              			
	$scope.predicate = 'age';
    $scope.reverse = true;
    $scope.order = function(predicate) {
        $scope.reverse = ($scope.predicate === predicate) ? !$scope.reverse : false;
        $scope.predicate = predicate;
	};
})




$scope.submitProduct = function () {
    console.log('before: ' + $scope.list.length);

    $scope.list.push({name: $scope.list.name, date: $scope.list.date, status: $scope.list.status, admin: $scope.list.admin, attend: $scope.list.attend, img: $scope.list.img});
    console.log('after:' + $scope.list.length);
    console.log($scope.list);

    $scope.showOverlay = false;

};





