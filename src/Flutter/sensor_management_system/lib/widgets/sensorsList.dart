import 'package:flutter/material.dart';
import 'package:flutter/widgets.dart';
import 'package:sensor_management_system/models/sensor.dart';
import 'package:sensor_management_system/models/sensorProperty.dart';
import 'package:sensor_management_system/services/webservice.dart';

class SensorsList extends StatefulWidget {
  @override
  State<StatefulWidget> createState() {
    return _SensorsListSate();
  }
}

class _SensorsListSate extends State<SensorsList> {
  List<Sensor> _sensors = List<Sensor>();
  List<SensorProperty> _sensorProperties = List<SensorProperty>();

  @override
  void initState() {
    super.initState();
    _populateSensors();
  }

  void _populateSensors() {
   
    WebService().load(Sensor.all).then((sensors) => {
      setState(() => {
        _sensors = sensors
      })
    });

    WebService().load(SensorProperty.all).then((sensorProperties) => {
      setState(() => {
        _sensorProperties = sensorProperties
      })
    });
  }

  ListTile _buildItemsForListView(BuildContext context, int index) {
    return ListTile(
      title: Text(_sensorProperties[index].measureType + _sensorProperties[index].measureUnit)
    );
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      body: ListView.builder(
        itemCount: _sensors.length,
        itemBuilder: _buildItemsForListView,
      ),
    );
  }
}