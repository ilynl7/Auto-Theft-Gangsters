using System.Collections.Generic;
using Sproto;

namespace SprotoType;

public class copy_scene_result
{
	public class request : SprotoTypeBase
	{
		private static int max_field_count = 9;

		private long _subType;

		private string _id;

		private bool _win;

		private long _gradeFlag;

		private long _grade;

		private List<item> _items;

		private long _swipe;

		private long _parm;

		private long _type;

		public long subType
		{
			get
			{
				return _subType;
			}
			set
			{
				has_field.set_field(0, is_has: true);
				_subType = value;
			}
		}

		public bool HasSubType => has_field.has_field(0);

		public string id
		{
			get
			{
				return _id;
			}
			set
			{
				has_field.set_field(1, is_has: true);
				_id = value;
			}
		}

		public bool HasId => has_field.has_field(1);

		public bool win
		{
			get
			{
				return _win;
			}
			set
			{
				has_field.set_field(2, is_has: true);
				_win = value;
			}
		}

		public bool HasWin => has_field.has_field(2);

		public long gradeFlag
		{
			get
			{
				return _gradeFlag;
			}
			set
			{
				has_field.set_field(3, is_has: true);
				_gradeFlag = value;
			}
		}

		public bool HasGradeFlag => has_field.has_field(3);

		public long grade
		{
			get
			{
				return _grade;
			}
			set
			{
				has_field.set_field(4, is_has: true);
				_grade = value;
			}
		}

		public bool HasGrade => has_field.has_field(4);

		public List<item> items
		{
			get
			{
				return _items;
			}
			set
			{
				has_field.set_field(5, is_has: true);
				_items = value;
			}
		}

		public bool HasItems => has_field.has_field(5);

		public long swipe
		{
			get
			{
				return _swipe;
			}
			set
			{
				has_field.set_field(6, is_has: true);
				_swipe = value;
			}
		}

		public bool HasSwipe => has_field.has_field(6);

		public long parm
		{
			get
			{
				return _parm;
			}
			set
			{
				has_field.set_field(7, is_has: true);
				_parm = value;
			}
		}

		public bool HasParm => has_field.has_field(7);

		public long type
		{
			get
			{
				return _type;
			}
			set
			{
				has_field.set_field(8, is_has: true);
				_type = value;
			}
		}

		public bool HasType => has_field.has_field(8);

		public request()
			: base(max_field_count)
		{
		}

		public request(byte[] buffer)
			: base(max_field_count, buffer)
		{
			decode();
		}

		protected override void decode()
		{
			int num = -1;
			while ((num = deserialize.read_tag()) != -1)
			{
				switch (num)
				{
				case 0:
					subType = deserialize.read_integer();
					break;
				case 1:
					id = deserialize.read_string();
					break;
				case 2:
					win = deserialize.read_boolean();
					break;
				case 3:
					gradeFlag = deserialize.read_integer();
					break;
				case 4:
					grade = deserialize.read_integer();
					break;
				case 5:
					items = deserialize.read_obj_list<item>();
					break;
				case 6:
					swipe = deserialize.read_integer();
					break;
				case 7:
					parm = deserialize.read_integer();
					break;
				case 8:
					type = deserialize.read_integer();
					break;
				default:
					deserialize.read_unknow_data();
					break;
				}
			}
		}

		public override int encode(SprotoStream stream)
		{
			serialize.open(stream);
			if (has_field.has_field(0))
			{
				serialize.write_integer(subType, 0);
			}
			if (has_field.has_field(1))
			{
				serialize.write_string(id, 1);
			}
			if (has_field.has_field(2))
			{
				serialize.write_boolean(win, 2);
			}
			if (has_field.has_field(3))
			{
				serialize.write_integer(gradeFlag, 3);
			}
			if (has_field.has_field(4))
			{
				serialize.write_integer(grade, 4);
			}
			if (has_field.has_field(5))
			{
				serialize.write_obj(items, 5);
			}
			if (has_field.has_field(6))
			{
				serialize.write_integer(swipe, 6);
			}
			if (has_field.has_field(7))
			{
				serialize.write_integer(parm, 7);
			}
			if (has_field.has_field(8))
			{
				serialize.write_integer(type, 8);
			}
			return serialize.close();
		}
	}
}
