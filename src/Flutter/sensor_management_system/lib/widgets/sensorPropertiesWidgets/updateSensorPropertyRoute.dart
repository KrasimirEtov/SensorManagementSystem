import 'package:flutter/cupertino.dart';
import 'package:flutter/material.dart';
import 'package:sensor_management_system/widgets/sensorPropertiesWidgets/updateSensorPropertyForm.dart';

class UpdateSensorPropertyRoute extends StatelessWidget {
  final String id;
  UpdateSensorPropertyRoute({this.id});

  @override
  Widget build(BuildContext context) {
    return Scaffold(
        appBar: AppBar(title: Text('Update Sensor Property')),
        body: UpdateSensorPropertyForm(id: this.id));
  }
}
