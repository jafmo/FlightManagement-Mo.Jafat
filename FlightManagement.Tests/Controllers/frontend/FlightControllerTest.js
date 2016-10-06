var data =
{
    newFlight:
        {
            data: {
                Id: 0, gateId: 1, code: "FL0", status: "Active",
                arrival: "1/10/2016 00:00",
                departure: "1/10/2016 00:29"
            }
        },
    flight:
        {
            data:
                {
                    success: true,
                    data: {
                        Id: 1, gateId: 1, code: "FL1", status: "Active",
                        arrival: "1/10/2016 00:00",
                        departure: "1/10/2016 00:29"
                    }
                }
        },
    flightsList:
        {
            success: true,
            data:
            [
            {
                Id: 1, gateId: 1, code: "FL1", status: "Active",
                arrival: "1/10/2016 00:00",
                departure: "1/10/2016 00:29"
            },
            {
                Id: 2, gateId: 1, code: "FL2", status: "Active",
                arrival: "1/10/2016 00:30",
                departure: "1/10/2016 00:59"
            }]
        }

};
describe('', function () {
    var $httpBackend,
     expectedUrl = 'http://localhost:50560/api/flights/',
     httpController, location;

    beforeEach(function () {
        module('FlightsApp');
    });

    describe('FlightJS', function () {
        beforeEach(inject(function ($rootScope, $controller, _$httpBackend_, $location) {
            $httpBackend = _$httpBackend_;
            scope = $rootScope.$new();
            location = $location;
            rootScope = $rootScope;
            httpController = $controller('FlightController', {
                '$scope': scope
            });
        }));

        it('$scope.go should change location to the path parameter', function () {
            spyOn(location, 'path');
            scope.go('/new/path');
            expect(location.path).toHaveBeenCalledWith('/new/path');
        });

        it('Title set to "Flights"', function () {
            expect(httpController).toBeDefined();
            expect(scope.Title).toEqual('Flights');
        });

        it('$scope.init should load list of flights and initialise selectedFlight, errorMessage', function () {
            scope.init();
            expect(scope.gates).toBeDefined();
            expect(scope.flights).toBeDefined();
            expect(scope.selectedFlight).toBeDefined();
            expect(scope.errorMessage).toBeDefined();
            expect(scope.selectedRowIndex).toBeDefined();
        });

        it('rootScope.selectedGateId change should call getAllFlights should load list of flights and initialise flights', function () {
            rootScope.selectedGateId = 1;
            $httpBackend.expectGET(expectedUrl + 'getAll?gateId=1').respond(200, data.flightsList.data);
            $httpBackend.flush();
            expect(scope.flights).toBeDefined();
        });

        it('$scope.setClickedRow to set selectedRowIndex andselectedFlightId"', function () {
            scope.setClickedRow(0, 1);
            expect(scope.selectedGateIndex).toEqual(0);
            expect(scope.selectedFlightId).toEqual(1);
        });

        it('$scope.loadEdit should call "getById" and set selected flight"', function () {
            $httpBackend.expectGET('http://localhost:50560/api/gates/getAll').respond(200, []);
            $httpBackend.expectGET(expectedUrl + 'getById?gateId=1&id=undefined').respond(200, data.flight.data.data);
            $httpBackend.expectGET(expectedUrl + 'getAll?gateId=1').respond(200, data.flightsList.data);
            scope.loadEdit();
            $httpBackend.flush();
            expect(httpController).toBeDefined();
            expect(scope.selectedFlight).toBeDefined();
            expect(scope.selectedFlight.Id).toEqual(1);
            expect(scope.selectedGateId).toBeDefined();
        });

        it('$scope.updateFlight calls "updateFlight" then return flight object.', function () {
            $httpBackend.expectPOST(expectedUrl + 'updateFlight?selectedGateId=1&pushOverlapForward=false').respond(200, data.flight.data);
            scope.selectedGateId = 1;
            $httpBackend.expectGET(expectedUrl + 'getAll?gateId=1').respond(200, data.flightsList.data);
            scope.pushOverlapForward = false;
            scope.updateFlight(data.flight.data.data);
            $httpBackend.flush();
            expect(httpController).toBeDefined();
            expect(scope.selectedFlight.Id).toEqual(1);
        });

        it('$scope.loadAdd calls "createNewFlight" then return flight object.', function () {
            $httpBackend.expectGET('http://localhost:50560/api/gates/getAll').respond(200, []);
            $httpBackend.expectGET(expectedUrl + 'CreateNewFlight?gateId=1').respond(200, data.newFlight.data);
            $httpBackend.expectGET(expectedUrl + 'getAll?gateId=1').respond(200, data.flightsList.data);
            scope.selectedGateId = 1;
            scope.loadAdd();
            $httpBackend.flush();
            expect(httpController).toBeDefined();
            expect(scope.gates).toBeDefined();
            expect(scope.selectedFlight).toBeDefined();
        });

        it('$scope.addFlight calls addFlight and set selectedFlight.', function () {
            $httpBackend.expectPOST(expectedUrl + 'addFlight').respond(200, data.flight.data);
            scope.selectedGateId = 1;
            $httpBackend.expectGET(expectedUrl + 'getAll?gateId=1').respond(200, data.flightsList.data);
            scope.addFlight(data.newFlight.data);
            $httpBackend.flush();
            expect(httpController).toBeDefined();
            expect(scope.selectedFlight).toBeDefined();
            expect(scope.selectedFlight.Id).toEqual(1);
        });

        it('$scope.deleteFlight calls "deleteFlight" then return flight object.', function () {
            $httpBackend.expectPOST(expectedUrl + 'deleteFlight?gateId=1&id=undefined').respond(200, data.flight.data);
            $httpBackend.expectGET(expectedUrl + 'getAll?gateId=1').respond(200, data.flightsList.data);
            scope.deleteFlight();
            $httpBackend.flush();
            expect(httpController).toBeDefined();
        });

        it('$scope.loadDelete calls "getById" then return flight object.', function () {
            $httpBackend.expectGET(expectedUrl + 'getById?gateId=1&id=undefined').respond(200, data.flight.data.data);
            $httpBackend.expectGET(expectedUrl + 'getAll?gateId=1').respond(200, data.flightsList.data);
            scope.loadDelete();
            $httpBackend.flush();
            expect(httpController).toBeDefined();
            expect(scope.selectedFlight).toBeDefined();
            expect(scope.selectedFlight.Id).toEqual(1);
        });
    });
});