import 'package:flutter/material.dart';
import 'package:flutter/widgets.dart';
import 'package:sensor_management_system/models/sensorProperty.dart';
import 'package:sensor_management_system/services/webservice.dart';
import 'package:sensor_management_system/widgets/sensorPropertiesWidgets/updateSensorPropertyRoute.dart';

class SensorPropertiesList extends StatefulWidget {
  @override
  State<StatefulWidget> createState() {
    return _SensorPropertiesListSate();
  }
}

class _SensorPropertiesListSate extends State<SensorPropertiesList> {
  List<SensorProperty> _sensorProperties = List<SensorProperty>();

  @override
  void initState() {
    super.initState();
    _populateSensorProperties();
  }

  void _populateSensorProperties() {
    WebService().load(SensorProperty.all).then((sensorProperties) => {
          setState(() => {_sensorProperties = sensorProperties})
        });
  }

  void _deleteSensorProperty(int index) {
    WebService()
        .delete(SensorProperty.initResourceByIdWithoutResponse(
            _sensorProperties[index].id))
        .whenComplete(() => {
              setState(() => {_sensorProperties.removeAt(index)})
            });
  }

  Card _buildItemsForListView(BuildContext context, int index) {
    return Card(
        child: ListTile(
      title: Text(_sensorProperties[index].measureType +
          ' - ' +
          _sensorProperties[index].measureUnit),
      subtitle: Text('Created on: ' + _sensorProperties[index].createdOn),
      trailing: PopupMenuButton(
          itemBuilder: (context) => [
                PopupMenuItem(
                    value: Text('Edit'),
                    child: ListTile(
                      leading: const Icon(Icons.edit),
                      title: Text('Edit'),
                      onTap: () {
                        Navigator.push(
                            context,
                            MaterialPageRoute(
                                builder: (context) => UpdateSensorPropertyRoute(
                                    id: _sensorProperties[index].id)));
                      },
                    )),
                PopupMenuItem(
                    value: Text('Remove'),
                    child: ListTile(
                      leading: const Icon(Icons.delete),
                      title: Text('Remove'),
                      onTap: () {
                        _deleteSensorProperty(index);
                      },
                    ))
              ]),
    ));
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
        body: Scrollbar(
            child: ListView.builder(
                itemCount: _sensorProperties.length,
                itemBuilder: _buildItemsForListView)));
  }
}
