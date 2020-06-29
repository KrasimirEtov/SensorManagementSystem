import 'dart:convert';
import 'package:sensor_management_system/services/webservice.dart';
import 'package:intl/intl.dart';

class Sensor {
  String id;
  String sensorPropertyId;
  String description;
  String pollingInterval;
  String minRangeValue;
  String maxRangeValue;
  String createdOn;

  Sensor(
      {this.id,
      this.sensorPropertyId,
      this.description,
      this.pollingInterval,
      this.minRangeValue,
      this.maxRangeValue,
      this.createdOn});

  factory Sensor.fromJson(Map<String, dynamic> json) {
    return new Sensor(
        id: json['id'].toString(),
        sensorPropertyId: json['sensorPropertyId'].toString(),
        description: json['description'].toString(),
        pollingInterval: json['pollingInterval'].toString(),
        minRangeValue: json['minRangeValue'].toString(),
        maxRangeValue: json['maxRangeValue'].toString(),
        createdOn: DateFormat("dd.MM.yyyy HH:mm:ss")
            .format(DateTime.parse(json['createdOn'])));
  }

   Map<String, dynamic> toJson() => {
        'id': id ?? "0",
        'sensorPropertyId': sensorPropertyId,
        'description': description,
        'pollingInterval': pollingInterval,
        'minRangeValue': minRangeValue,
        'maxRangeValue': maxRangeValue
      };

  static Resource<List<Sensor>> get all {
    return Resource(
        url: 'http://192.168.1.4:5003/api/sensor/all',
        parse: (response) {
          final result = json.decode(response.body);
          Iterable list = result;
          return list.map((model) => Sensor.fromJson(model)).toList();
        });
  }

  static Resource<Sensor> initResourceByIdWithoutResponse(String id) {
    return Resource.urlOnly(
        url: 'http://192.168.1.4:5003/api/sensor/' + id);
  }

  static Resource<Sensor> initResourceByIdWithResponse(String id) {
    return Resource(
        url: 'http://192.168.1.4:5003/api/sensor/' + id,
        parse: (response) {
          final result = json.decode(response.body);
          return Sensor.fromJson(result);
        });
  }

  static Resource<Sensor> initWithJsonBody(Sensor payload) {
    return Resource.withJsonBody(
        'http://192.168.1.4:5003/api/sensor/', payload.toJson());
  }
}
