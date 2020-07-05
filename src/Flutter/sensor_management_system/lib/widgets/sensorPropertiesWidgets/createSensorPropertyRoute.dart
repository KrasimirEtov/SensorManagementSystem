import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:sensor_management_system/widgets/sensorPropertiesWidgets/createSensorPropertyForm.dart';

class CreateSensorPropertyRoute extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
        appBar: AppBar(title: Text('Create Sensor Property')),
        body: CreateSensorPropertyForm());
  }
}
