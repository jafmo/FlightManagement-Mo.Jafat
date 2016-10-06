FlightsApp.service('GateService', ['$http', '$httpParamSerializerJQLike',
    function ($http, $httpParamSerializerJQLike) {
        var baseUrl = "http://localhost:50560/api/gates/";
        //Create new record
        this.addGate = function (gate) {
            var request = $http({
                method: "POST",
                url: baseUrl + "addGate/",
                data: $httpParamSerializerJQLike(gate),
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded'
                }
            });
            return request;
        }
        this.createNewGate = function () {
            return $http.get(baseUrl + "CreateNewGate");
        }
        //Get Single Record
        this.getById = function (id) {
            return $http.get(baseUrl + "getById/" + id);
        }

        //Get All 
        this.getAllGates = function () {
            return $http.get(baseUrl + "getAll");
        }
        //Update the Record
        this.updateGate = function (gate) {
            var request = $http({
                method: "POST",
                url: baseUrl + "updateGate",
                data: $httpParamSerializerJQLike(gate),
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded'
                }
            });
            return request;
        }
        //Delete the Record
        this.deleteGate = function (id) {
            var request = $http({
                method: "POST",
                url: baseUrl + "deleteGate/" + id
            });
            return request;
        }
    }]);