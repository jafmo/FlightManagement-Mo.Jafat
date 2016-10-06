FlightsApp.service('FlightService', ['$http', '$httpParamSerializerJQLike',
    function ($http, $httpParamSerializerJQLike) {
        var baseUrl = "http://localhost:50560/api/flights/";

        //Create new record
        this.addFlight = function (flight) {
            // displayed date format does not deserialize correctly so I had to get pure date object
            flight.arrival = strToDate(flight.arrival);
            flight.departure = strToDate(flight.departure);
            var request = $http({
                method: "POST",
                url: baseUrl + "addFlight",
                data: $httpParamSerializerJQLike(flight),
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded'
                }
            });
            return request;
        }
        this.createNewFlight = function (gateId) {
            return $http.get(baseUrl + "CreateNewFlight?gateId=" + gateId);
        }
        //Get Single Record
        this.getById = function (gateId,id) {
            return $http.get(baseUrl + "getById?gateId=" + gateId + "&id=" + id);
        }

        //Get All 
        this.getAllFlights = function (gateId) {
            return $http.get(baseUrl + "getAll?gateId=" + gateId);
        }
        //Update the Record
        this.updateFlight = function (gateId, flight, pushOverlapForward) {
            // displayed date format does not deserialize correctly so I had to get pure date object
            flight.arrival = strToDate(flight.arrival);
            flight.departure = strToDate(flight.departure);
            var request = $http({
                method: "POST",
                url: baseUrl + "updateFlight?selectedGateId=" + gateId + "&pushOverlapForward=" + pushOverlapForward,
                data: $httpParamSerializerJQLike(flight),
                headers: {
                    'Content-Type': 'application/x-www-form-urlencoded'
                }
            });
            return request;
        }
        //Delete the Record
        this.deleteFlight = function (gateId,id) {
            var request = $http({
                method: "POST",
                url: baseUrl + "deleteFlight?gateId=" + gateId + "&id=" + id
            });
            return request;
        }
        // date string to date object
        var strToDate = function (dateTimeStr) {
            if (dateTimeStr) {
                var dateArr = String(dateTimeStr).split(" ")[0].split("/");
                var timeArr = String(dateTimeStr).split(" ")[1].split(":");
                var newDateTime = new Date(parseInt(dateArr[2]), parseInt(dateArr[1]) - 1, parseInt(dateArr[0]), parseInt(timeArr[0]), parseInt(timeArr[1]), 0, 0);
                return newDateTime;
            }
        }
    }]);