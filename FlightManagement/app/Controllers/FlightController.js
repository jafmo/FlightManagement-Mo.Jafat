FlightsApp.controller('FlightController', function ($rootScope, $scope, $location, $routeParams,$filter, FlightService, GateService) {
    $scope.Title = "Flights";
    $scope.init = function () {
        $scope.flights = [];
        $scope.gates = [];
        $scope.selectedRowIndex = null;
        $scope.selectedFlight = null;
        $scope.errorMessage = "";
    }
    $rootScope.$watch('selectedGateId', function () {
        getAllFlights();
    });
    $scope.$watch('selectedFlight.arrival', function (Value) {
        if ($scope.selectedFlight) {
            $scope.selectedFlight.arrival = $filter('date')(Value, 'dd/MM/yyyy HH:mm a');
        }
    });
    $scope.$watch('selectedFlight.departure', function (Value) {
        if ($scope.selectedFlight) {
            $scope.selectedFlight.departure = $filter('date')(Value, 'dd/MM/yyyy HH:mm a');
        }
    });
    var getAllGates = function()
    {
        GateService.getAllGates().then(function (gates) {
            $scope.gates = gates.data;
        });
    }
    var getAllFlights = function () {
        FlightService.getAllFlights($rootScope.selectedGateId).then(function (flights) {
            $scope.flights = flights.data
        });
    }
    $scope.setClickedRow = function (index, flightId) {
        $scope.selectedRowIndex = index;
        $rootScope.selectedFlightId = flightId;
    }
    //*****//
    $scope.go = function (path) {
        $location.path(path);
    };
    $scope.goBack = function () {
        history.back();
    }
    //******//
    $scope.loadEdit = function () {
        getAllGates();
        $scope.pushOverlapForward = false;
        var id = $routeParams.id;
        FlightService.getById($rootScope.selectedGateId, id).then(function (returnedFlight) {
            $scope.selectedFlight = returnedFlight.data;
            $scope.selectedGateId = returnedFlight.data.gateId.toString();
        });
    }
    $scope.updateFlight = function (flight) {
        FlightService.updateFlight(parseInt($scope.selectedGateId), flight, $scope.pushOverlapForward).then(function (returnedFlight) {
            $scope.selectedFlight = returnedFlight.data.data;
            if (returnedFlight.data.success == true) {
                $scope.go('/');
            }
            else {
                $scope.errorMessage = returnedFlight.data.error;
            }
        });
    }
    $scope.addFlight = function (flight) {
        // assign flight to selected gate, in case user selects different gate
        flight.gateId = parseInt($scope.selectedGateId);
        FlightService.addFlight(flight).then(function (returnedFlight) {
            $scope.selectedFlight = returnedFlight.data.data;
            if (returnedFlight.data.success == true) {
                $scope.go('/');
            }
            else {
                $scope.errorMessage = returnedFlight.data.error;
            }
        });
    }
    $scope.deleteFlight = function (id) {
        FlightService.deleteFlight($rootScope.selectedGateId,id).then(function (result) {
            if (result) {
                $scope.go('/');
            }
        });
    }
    $scope.loadDelete = function () {
        var id = $routeParams.id;
        FlightService.getById($rootScope.selectedGateId,id).then(function (flight) {
            $scope.selectedFlight = flight.data;
        });
    }
    $scope.loadAdd = function () {
        // get selectedGateId from root to select gate dropdown
        $scope.selectedGateId = $rootScope.selectedGateId.toString();
        getAllGates();
        $scope.createNewFlight();
    }
    $scope.createNewFlight = function () {
        // return flight object for selected gate
        FlightService.createNewFlight(parseInt($scope.selectedGateId)).then(function (flight) {
            $scope.selectedFlight = flight.data;
        });
    }
});