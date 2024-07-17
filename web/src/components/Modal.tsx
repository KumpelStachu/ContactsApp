import clsx from 'clsx'
import Portal from './Portal'
import { XIcon } from 'lucide-react'
import { useEffect, useRef, useState } from 'react'

export default function Modal({
	children,
	title,
	isOpen,
	onClose,
}: {
	children: React.ReactNode
	title?: string
	isOpen?: boolean
	onClose?: () => void
}) {
	const ref = useRef<HTMLDivElement>(null)
	const [showChildren, setShowChildren] = useState(isOpen)

	useEffect(() => {
		if (isOpen) {
			setShowChildren(true)
		} else {
			const timeout = setTimeout(() => setShowChildren(false), 200)
			return () => clearTimeout(timeout)
		}
	}, [isOpen])

	return (
		<Portal>
			<div
				ref={ref}
				className={clsx('modal', isOpen && 'modal-open')}
				onClick={e => {
					if (e.target === ref.current) {
						onClose?.()
					}
				}}
			>
				<div className="modal-box">
					{title && (
						<div className="flex justify-between mb-4">
							<span className="text-xl font-bold">{title}</span>
							<button className="btn btn-square btn-sm btn-ghost">
								<XIcon className="w-6 h-6" onClick={onClose} />
							</button>
						</div>
					)}
					{showChildren && children}
				</div>
			</div>
		</Portal>
	)
}
