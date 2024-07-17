import useUser from '@/hooks/useUser'
import { Link } from '@tanstack/react-router'
import { User2Icon } from 'lucide-react'

export default function Navbar() {
	const { user } = useUser()

	return (
		<div className="top-2 container sticky z-10 mx-auto">
			<div className="navbar bg-primary text-primary-content rounded-lg">
				<div className="navbar-start">
					<Link to="/" className="btn btn-ghost">
						Lista kontaktów
					</Link>

					{user && (
						<Link to="/contact/create" className="btn btn-ghost">
							Dodaj kontakt
						</Link>
					)}
				</div>
				<div className="navbar-end">
					<div className="dropdown dropdown-end">
						<div tabIndex={0} className="btn btn-ghost m-1">
							<User2Icon className="w-6 h-6" />
						</div>
						<ul
							tabIndex={0}
							className="menu dropdown-content bg-base-100 text-base-content rounded-box w-52 p-2 shadow-md z-[1]"
						>
							{user ? (
								<>
									<li>
										<Link to="/contact/$contactId" params={{ contactId: user.id.toString() }}>
											Profil
										</Link>
									</li>
									<li>
										<Link to="/logout">Wyloguj się</Link>
									</li>
								</>
							) : (
								<>
									<li>
										<Link to="/login">Zaloguj się</Link>
										<Link to="/register">Zarejestruj się</Link>
									</li>
								</>
							)}
						</ul>
					</div>
				</div>
			</div>
		</div>
	)
}
