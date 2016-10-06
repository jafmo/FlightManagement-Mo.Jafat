FlightsApp.controller('GateController', function ($rootScope,$scope, $location, $routeParams, GateService) {
    $scope.Title = "Gates";
    $scope.init = function () {
        $scope.gates = [];
        $scope.selectedRowIndex = $rootScope.selectedGateIndex;
        getAllGates();
        $scope.selectedGate = null;
        $scope.errorMessage = "";
    }
    var getAllGates = function () {
        GateService.getAllGates().then(function (gates) {
            $scope.gates = gates.data;
            $rootScope.gates = gates.data;
            $rootScope.selectedGateCode = $scope.gates[0].code;
        });
    }
    $scope.setClickedRow = function (index, gateId,gateCode) {
        $scope.selectedRowIndex = index;
        $rootScope.selectedGateIndex = index; // track selected gate row index
        $rootScope.selectedGateId = gateId;
        $rootScope.selectedGateCode = gateCode
    }
    $scope.go = function (path) {
        $location.path(path);
    };
    $scope.goBack = function () {
        history.back();
    }
    $scope.loadEdit = function () {
        var id = $routeParams.id;
        GateService.getById(id).then(function (gate) {
            $scope.selectedGate = gate.data;
        });
    }
    $scope.updateGate = function (gate) {
        GateService.updateGate(gate).then(function (gate) {
            $scope.selectedGate = gate.data.data;
            if (gate.data.success == true) {
                $scope.go('/');
            }
            else {
                $scope.errorMessage = gate.data.error;
            }
        });
    }
    $scope.addGate = function (gate) {
        GateService.addGate(gate).then(function (gate) {
            $scope.selectedGate = gate.data.data;
            if (gate.data.success == true) {
                $scope.go('/');
            }
            else {
                $scope.errorMessage = gate.data.error;
            }
        });
    }
    $scope.deleteGate = function (id) {
        GateService.deleteGate(id).then(function (result) {
            if (result) {
                $scope.go('/');
            }
        });
    }
    $scope.loadDelete = function () {
        var id = $routeParams.id;
        GateService.getById(id).then(function (gate) {
            $scope.selectedGate = gate.data;
        });
    }
    $scope.loadAdd = function () {
        $scope.createNewGate();
    }
    $scope.createNewGate = function()
    {
        GateService.createNewGate().then(function (gate) {
            $scope.selectedGate = gate.data;
        });
    }
});