var data =
{
    newGate:
        {
            data: { Id: 0, code: "G0", status: "Open", flights: [] }
        },
    gate:
        {
            data:
                {
                    success: true,
                    data: { Id: 3, code: "G3", status: "Open", flights: [] }
                }
        },
    gatesList:
        {
            success: true,
            data:
            [
            { Id: 1, code: "G1", status: "Open", flights: [] },
            { Id: 2, code: "G2", status: "Open", flights: [] }
            ]
        }

};
describe('', function () {
    var $httpBackend,
     expectedUrl = 'http://localhost:50560/api/gates/',
     httpController, location;

    beforeEach(function () {
        module('FlightsApp');
    });

    describe('GateJS', function () {
        beforeEach(inject(function ($rootScope, $controller, _$httpBackend_, $location) {
            $httpBackend = _$httpBackend_;
            scope = $rootScope.$new();
            location = $location;
            rootScope = $rootScope;
            httpController = $controller('GateController', {
                '$scope': scope
            });
        }));

        it('$scope.go should change location to the path parameter', function () {
            spyOn(location, 'path');
            scope.go('/new/path');
            expect(location.path).toHaveBeenCalledWith('/new/path');
        });

        it('Title set to "Gates"', function () {
            expect(httpController).toBeDefined();
            expect(scope.Title).toEqual('Gates');
        });

        it('$scope.init should load list of gates and initialise selectedGate, errorMessage', function () {
            $httpBackend.expectGET(expectedUrl + 'getAll').respond(200, data.gatesList.data);
            rootScope.selectedGateIndex = 0;
            scope.init();
            $httpBackend.flush();
            expect(httpController).toBeDefined();
            expect(scope.gates).toBeDefined();
            expect(scope.gates.length).toEqual(2);
            expect(scope.selectedGate).toBeDefined();
            expect(scope.errorMessage).toBeDefined();
            expect(scope.selectedRowIndex).toBeDefined();
            expect(scope.selectedRowIndex).toEqual(0);
        });

        it('$scope.setClickedRow to set selectedRowIndex, gateId and gate Code"', function () {
            scope.setClickedRow(0, 1, "G1");
            expect(scope.selectedGateIndex).toEqual(0);
            expect(scope.selectedGateId).toEqual(1);
            expect(scope.selectedGateCode).toEqual("G1");
        });

        it('$scope.loadEdit should call "getById" and set selected gate"', function () {
            $httpBackend.expectGET(expectedUrl + 'getById/undefined').respond(200, data.gate.data);
            scope.loadEdit();
            $httpBackend.flush();
            expect(httpController).toBeDefined();
            expect(scope.selectedGate).toBeDefined();
            expect(scope.selectedGate.data.Id).toEqual(3);
        });

        it('$scope.updateGate calls "updateGate" then return gate object.', function () {
            $httpBackend.expectPOST(expectedUrl + 'updateGate').respond(200, data.gate.data);
            scope.updateGate();
            $httpBackend.flush();
            expect(httpController).toBeDefined();
            expect(scope.selectedGate.Id).toEqual(3);
        });

        it('$scope.loadAdd calls "createNewGate" then return gate object.', function () {
            $httpBackend.expectGET(expectedUrl + 'CreateNewGate').respond(200, data.newGate.data);
            scope.loadAdd();
            $httpBackend.flush();
            expect(httpController).toBeDefined();
            expect(scope.selectedGate.Id).toEqual(0);
        });

        it('$scope.addGate calls addGate and set selectedGate.', function () {
            $httpBackend.expectPOST(expectedUrl + 'addGate/').respond(200, data.gate.data);
            scope.addGate();
            $httpBackend.flush();
            expect(httpController).toBeDefined();
            expect(scope.selectedGate).toBeDefined();
            expect(scope.selectedGate.Id).toEqual(3);
        });

        it('$scope.deleteGate calls "deleteGate" then return gate object.', function () {
            $httpBackend.expectPOST(expectedUrl + 'deleteGate/undefined').respond(200, data.gate.data);
            scope.deleteGate();
            $httpBackend.flush();
            expect(httpController).toBeDefined();
        });

        it('$scope.loadDelete calls "getById" then return gate object.', function () {
            $httpBackend.expectGET(expectedUrl + 'getById/undefined').respond(200, data.gate.data);
            scope.loadDelete();
            $httpBackend.flush();
            expect(httpController).toBeDefined();
            expect(scope.selectedGate).toBeDefined();
            expect(scope.selectedGate.data.Id).toEqual(3);
        });

    });
});