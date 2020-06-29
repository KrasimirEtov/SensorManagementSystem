import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:sensor_management_system/widgets/sensorWidgets/createSensorForm.dart';

class CreateSensorRoute extends StatelessWidget {
  @override
  Widget build(BuildContext context) {
    return Scaffold(
        appBar: AppBar(title: Text('Create Sensor')),
        body: CreateSensorForm());
  }
}
