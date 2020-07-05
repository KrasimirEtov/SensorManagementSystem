import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:sensor_management_system/widgets/sensorWidgets/updateSensorForm.dart';

class UpdateSensorRoute extends StatelessWidget {
  final String id;
  UpdateSensorRoute({this.id});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
        appBar: AppBar(title: Text('Update Sensor')),
        body: UpdateSensorForm(id: this.id));
  }
}
