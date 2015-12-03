
----select tbl.engeneer, sum(cost_people), sum(price)
----from (
--select cam.id_service_came, ctr.full_name, cl_cat.number, at.name, dbo.srvpl_get_device_name(cl.id_device, 'no_serial_num') as device, 
----class.price, class.cost_people, 
--u_eng.display_name as engeneer, u_adm.display_name as [admin], cam.date_came
--from srvpl_service_cames cam inner join 
--srvpl_service_claims cl on cam.id_service_claim=cl.id_service_claim
--inner join srvpl_contracts c on c.id_contract=cl.id_contract
--inner join get_contractor(null) ctr on ctr.id=c.id_contractor
--inner join srvpl_contract2devices c2d on c2d.id_contract2devices=cl.id_contract2devices
--inner join srvpl_devices d on cl.id_device=d.id_device
--inner join srvpl_device_models dm on d.id_device_model=dm.id_device_model
--inner join Service.dbo.classifier_categories cl_cat on cl_cat.id=dm.id_classifier_category
----left join Service.dbo.classifier class on class.id_category = cl_cat.id
--inner join srvpl_service_action_types at on cam.id_service_action_type=at.id_service_action_type
--inner join users u_eng on u_eng.id_user=cl.id_service_engeneer
--inner join users u_adm on u_adm.id_user=cl.id_service_admin
--where cl.enabled=1 and cam.enabled=1 and c.enabled = 1
--and c2d.enabled=1 and cl_cat.enabled=1 
----and class.enabled=1
--and Year(date_came) = 2015 and Month(date_came) = 11
----) as tbl
----group by tbl.engeneer
--3040

--select tbl.engeneer, sum(cost_people), sum(price)
--from (

select cam.id_service_came, ctr.full_name, c.number, cl_cat.number, at.name, dbo.srvpl_get_device_name(cl.id_device, 'no_serial_num') as device,  
class.price, class.cost_people, 
u_eng.display_name as engeneer, u_adm.display_name as [admin], cam.date_came
from srvpl_service_cames cam inner join 
srvpl_service_claims cl on cam.id_service_claim=cl.id_service_claim
inner join srvpl_contracts c on c.id_contract=cl.id_contract
inner join get_contractor(null) ctr on ctr.id=c.id_contractor
inner join srvpl_contract2devices c2d on c2d.id_contract2devices=cl.id_contract2devices
inner join srvpl_devices d on cl.id_device=d.id_device
inner join srvpl_device_models dm on d.id_device_model=dm.id_device_model
inner join Service.dbo.classifier_categories cl_cat on cl_cat.id=dm.id_classifier_category
left join srvpl_service_action_types at on cam.id_service_action_type=at.id_service_action_type
inner join Service.dbo.classifier class on class.id_category = cl_cat.id 
and id_work_type = 
case when at.id_service_action_type = 1 then 2 else 
case when at.id_service_action_type = 2 then 7 else 
case when at.id_service_action_type = 3 then 6 else 
case when at.id_service_action_type = 4 then 5 else 0 
end 
end 
end 
end
left join users u_eng on u_eng.id_user=cl.id_service_engeneer
left join users u_adm on u_adm.id_user=cl.id_service_admin
where cam.enabled=1 and cl.enabled=1 and cam.enabled=1 and c.enabled = 1 and c2d.enabled=1 and Year(date_came) = 2015 and Month(date_came) = 11
--) as tbl
--group by tbl.engeneer