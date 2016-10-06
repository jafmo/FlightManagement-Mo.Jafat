var FlightsApp = angular.module('FlightsApp', ['ngResource', 'ngRoute', 'moment-picker'])
.run(function ($rootScope) {
    $rootScope.currentDate = new Date();
    $rootScope.selectedGateIndex = 0;
    $rootScope.selectedGateId = 1;
    $rootScope.selectedGateCode = "";
    $rootScope.selectedFlightId = 0;
});

// configure date/time picker
FlightsApp.config(['momentPickerProvider', function (momentPickerProvider) {
    momentPickerProvider.options({

    });
}]);

// configure our routes
FlightsApp.config(['$routeProvider', function ($routeProvider) {
    $routeProvider
        // route for the home page
        .when('/', {
            templateUrl: 'views/home.html'
        })

        // route for Edit
        .when('/gate/:id/Edit', {
            templateUrl: 'views/gate/Edit.html',
            controller: 'GateController'
        })

        // route for Delete
        .when('/gate/:id/Delete', {
            templateUrl: 'views/gate/Delete.html',
            controller: 'GateController'
        })

        // route for Add
        .when('/gate/Add', {
            templateUrl: 'views/gate/Add.html',
            controller: 'GateController'
        })

        // route for flights 

        // route for Edit
        .when('/flight/:id/Edit', {
            templateUrl: 'views/flight/Edit.html',
            controller: 'FlightController'
        })

        // route for Delete
        .when('/flight/:id/Delete', {
            templateUrl: 'views/flight/Delete.html',
            controller: 'FlightController'
        })

        // route for Add
        .when('/flight/Add', {
            templateUrl: 'views/flight/Add.html',
            controller: 'FlightController'
        });
    
}
]);