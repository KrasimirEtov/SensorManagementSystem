import 'dart:convert';
import 'package:sensor_management_system/services/webservice.dart';
import 'package:intl/intl.dart';

class SensorProperty {
  String id;
  String measureType;
  String measureUnit;
  String isSwitch;
  String createdOn;

  SensorProperty(
      {this.id,
      this.measureType,
      this.measureUnit,
      this.isSwitch,
      this.createdOn});

  factory SensorProperty.fromJson(Map<String, dynamic> json) {
    return new SensorProperty(
        id: json['id'].toString(),
        measureType: json['measureType'].toString(),
        measureUnit: json['measureUnit'].toString(),
        isSwitch: json['isSwitch'].toString(),
        createdOn: DateFormat("dd.MM.yyyy HH:mm:ss")
            .format(DateTime.parse(json['createdOn'])));
  }

  Map<String, dynamic> toJson() => {
        'id': id ?? "0",
        'measureType': measureType,
        'measureUnit': measureUnit,
        'isSwitch': isSwitch ?? "false"
      };

  static Resource<List<SensorProperty>> get all {
    return Resource(
        url: 'http://192.168.1.4:5003/api/sensorproperty/all',
        parse: (response) {
          final result = json.decode(response.body);
          Iterable list = result;
          return list.map((model) => SensorProperty.fromJson(model)).toList();
        });
  }

  static Resource<SensorProperty> initResourceById(String id) {
    return Resource.urlOnly(
        url: 'http://192.168.1.4:5003/api/sensorproperty/' + id);
  }

  static Resource<SensorProperty> initWithJsonBody(SensorProperty payload) {
    return Resource.withJsonBody(
        'http://192.168.1.4:5003/api/sensorproperty/', payload.toJson());
  }
}
