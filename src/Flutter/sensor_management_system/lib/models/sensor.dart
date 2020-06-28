import 'dart:convert';
import 'package:sensor_management_system/services/webservice.dart';


class Sensor {
  final String id;
  final String sensorPropertyId;
  final String description;
  final String pollingInterval;
  final String minRangeValue;
  final String maxRangeValue;

  Sensor({this.id, this.sensorPropertyId, this.description, this.pollingInterval, this.minRangeValue, this.maxRangeValue});

  factory Sensor.fromJson(Map<String, dynamic> json) {
    return new Sensor(
      id: json['id'].toString(),
      sensorPropertyId: json['sensorPropertyId'].toString(),
      description: json['description'].toString(),
      pollingInterval: json['pollingInterval'].toString(),
      minRangeValue: json['minRangeValue'].toString(),
      maxRangeValue: json['maxRangeValue'].toString()
    );
  }

  static Resource<List<Sensor>> get all {
    return Resource(
      url: 'http://192.168.1.4:5003/api/sensor/all',
      parse: (response) {
        final result = json.decode(response.body);
        Iterable list = result;
        return list.map((model) => Sensor.fromJson(model)).toList();
      }
    );
  }
}